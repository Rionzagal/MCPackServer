using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCPackServer.Pages.ProjectsModule
{
    public partial class ProjectProductsDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public ProjectProducts Model { get; set; }
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        private bool SearchForCode;
        #endregion

        #region API elements
        private MudForm Form;
        private List<MCProducts> ProductsList = new();
        private List<ProjectProducts> projectProducts = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = $"Añadir nuevo producto al proyecto {Model.ProjectId}";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
                await GetProjectProducts();
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar proyecto seleccionado";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar proyecto seleccionado";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else Dialog.Cancel();
        }

        private async Task Submit()
        {
            _processing = true;
            ActionResponse<ProjectProducts> response = new();
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State) response = await _productsService.AddAsync(Model);
                else if (States.Edit == State) response = await _productsService.UpdateAsync(Model);
                else if (States.Delete == State) response = await _productsService.RemoveAsync(Model);
                Dialog.Close(DialogResult.Ok(response));
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }

        private async Task<IEnumerable<int>> ProductsServerReload(string filter, bool UseCode = false)
        {
            List<int> result = new();
            List<WhereFilter> filters = new()
            {
                new WhereFilter
                {
                    Field = UseCode ? "Code" : "Description",
                    Value = filter
                }
            };
            DataManagerRequest dm = new()
            {
                Where = filters,
            };
            var response = await _service.GetForGridAsync<MCProducts>(dm);
            if (null != response)
            {
                ProductsList = response.ToList();
                ProductsList.ForEach(product => result.Add(product.Id));
            }
            return result;
        }

        private async Task GetProjectProducts()
        {
            DataManagerRequest dm = new()
            {
                Where = new List<WhereFilter>()
                {
                    new WhereFilter { Field = "ProjectId", Value = Model.ProjectId.ToString() }
                }
            };
            var response = await _productsService.GetForGridAsync<ProjectProducts>(dm);
            if (null != response) projectProducts = response.ToList();
        }

        private string GetProductCode(int Id)
        {
            string result = "";
            if (0 != Id)
            {
                var match = ProductsList.FirstOrDefault(p => Id == p.Id);
                if (null != match) result = match.Code;
            }
            return result;
        }

        private string GetProductDescription(int Id)
        {
            string result = "";
            if (0 != Id)
            {
                var match = ProductsList.FirstOrDefault(p => Id == p.Id);
                if (null != match) result = match.Description;
            }
            return result;
        }
    }
}
