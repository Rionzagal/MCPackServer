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
using Newtonsoft.Json;

namespace MCPackServer.Pages.PurchaseOrdersModule
{
    public partial class ArticlesToPurchaseDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public ArticlesToPurchaseView? ModelView { get; set; }
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private MudForm Form = new();
        #endregion

        private ArticlesToPurchase Model = new();
        private PurchaseOrdersView Reference = new();

        protected override async Task OnParametersSetAsync()
        {
            #region Parameters checking and reference recovery
            if (ModelView == null)
                Dialog?.Cancel();
            else
            {
                Reference = await _service.GetByKeyAsync<PurchaseOrdersView>
                    (ModelView.PurchaseOrderId, nameof(PurchaseOrdersView.Id));
                DataManagerRequest request = new()
                {
                    Take = 1,
                    Where = new List<WhereFilter>()
                    {
                        new WhereFilter { Field = nameof(ArticlesToPurchase.PurchaseOrderId), Value = Reference.Id.ToString() },
                        new WhereFilter { Field = nameof(ArticlesToPurchase.QuoteId), Value = ModelView.QuoteId.ToString() }
                    }
                };
                Model = (await _service.GetForGridAsync<ArticlesToPurchase>(request, nameof(ArticlesToPurchase.QuoteId)))
                    .First();
            }
            #endregion
        }

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nuevo artículo a OC";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar artículo de OC seleccionado";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar artículo de OC seleccionado";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else //should not get to this option
            {
                Dialog?.Cancel();
            }
        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Edit == State)
                {
                    var response = await _service.UpdateAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Artículo de compra actualizado con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                }
                else if (States.Delete == State)
                {
                    var response = await _service.RemoveAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Artículo de compra eliminado con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                }
                _processing = false;
                Dialog?.Close();
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }
    }
}
