using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCPackServer.Entities;
using MCPackServer.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace MCPackServer.Pages.PurchaseOrdersModule
{
    public partial class ArticlesToPurchaseDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public PurchaseOrders Reference { get; set; }
        [Parameter]
        public ArticlesToPurchase Model { get; set; } = new();
        #endregion

        #region Dialog variables
        private string Title;
        private string TitleIcon;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private MudForm Form;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nuevo proyecto";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
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
            else //should not get to this option
            {
                Dialog.Cancel();
            }
        }

        private async Task Submit()
        {
            _processing = true;
            ActionResponse<ArticlesToPurchase> response = new();
            await Form.Validate();
            if (Form.IsValid)
            {
                ArticlesToPurchase element = new();
                if (States.Add == State) response = await _articlesService.AddAsync(Model);
                else if (States.Edit == State) response = await _articlesService.UpdateAsync(Model);
                else if (States.Delete == State) response = await _articlesService.RemoveAsync(Model);
                if (response.IsSuccessful)
                {
                    element = response.Value;
                }
                var result = new
                {
                    Success = response.IsSuccessful,
                    Value = element
                };
                _processing = false;
                Dialog.Close(DialogResult.Ok(JsonSerializer.Serialize(result)));
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }
    }
}
