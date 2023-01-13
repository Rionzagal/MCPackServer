using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
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
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public ProjectProductsView? ModelView { get; set; }
        [Parameter]
        public int? ProjectId { get; set; }
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
        private MudForm Form = new();
        private List<MCProducts> ProductsList = new();
        private List<ProjectProductsView> projectProducts = new();
        #endregion

        private ProjectProducts Model = new();

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = $"Añadir nuevo producto a proyecto seleccionado";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
                await GetProjectProducts();
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar producto de proyecto";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar producto seleccionado de proyecto";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else
                Dialog?.Cancel();
            if (null != ModelView)
            {
                //var propertes = typeof(ProjectProducts).GetProperties()
                //    .Where(p => p.GetAccessors()[0].IsFinal || !p.GetAccessors()[0].IsVirtual)
                //    .ToList();
                //foreach (var item in propertes)
                //{
                //    var value = item.GetValue(ModelView);
                //    if (null != value)
                //        item.SetValue(Model, value);
                //}
                DataManagerRequest dm = new()
                {
                    Take = 1,
                    Skip = 0,
                    Where = new List<WhereFilter>
                    {
                        new WhereFilter
                        {
                            Field = nameof(Entities.ProjectProducts.ProjectId),
                            Value = ModelView.ProjectId,
                            Operator = Operators.Equal
                        },
                        new WhereFilter
                        {
                            Field = nameof(Entities.ProjectProducts.ProductId),
                            Value = ModelView.ProductId,
                            Operator = Operators.Equal
                        }
                    }
                };
                Model = (await _service.GetForGridAsync<ProjectProducts>(dm, "ProductId")).First();
            }
            else if (ProjectId.HasValue)
                Model.ProjectId = ProjectId.Value;
        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State)
                {
                    var response = await _service.AddAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Producto añadido exitosamente al proyecto.", Severity.Success);
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    }
                }
                else if (States.Edit == State)
                {
                    var response = await _service.UpdateAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Producto editado exitosamente en el proyecto.", Severity.Success);
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    }
                }
                else if (States.Delete == State)
                {
                    var response = await _service.RemoveAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Producto eliminado exitosamente del proyecto.", Severity.Success);
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    }
                }
                Dialog?.Close();
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
                    Value = filter,
                    Operator = Operators.StartsWith
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
                result = ProductsList.Select(p => p.Id).ToList();
            }
            return result;
        }

        private async Task GetProjectProducts()
        {
            DataManagerRequest dm = new()
            {
                Where = new List<WhereFilter>()
                {
                    new WhereFilter
                    {
                        Field = "ProjectId",
                        Value = Model.ProjectId,
                        Operator = Operators.Equal
                    }
                }
            };
            var response = await _service.GetForGridAsync<ProjectProductsView>(dm, "ProductId");
            if (null != response)
                projectProducts = response.ToList();
        }

        private string GetProductCode(int Id)
        {
            string result = "";
            if (0 != Id)
            {
                var match = ProductsList.FirstOrDefault(p => Id == p.Id);
                if (null != match)
                    result = match.Code;
            }
            return result;
        }

        private string GetProductDescription(int Id)
        {
            string result = "";
            if (0 != Id)
            {
                var match = ProductsList.FirstOrDefault(p => Id == p.Id);
                if (null != match)
                    result = match.Description;
            }
            return result;
        }
    }
}
