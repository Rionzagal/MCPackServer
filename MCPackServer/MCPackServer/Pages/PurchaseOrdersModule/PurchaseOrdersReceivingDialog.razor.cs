using MCPackServer.Models;
using MCPackServer.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;

namespace MCPackServer.Pages.PurchaseOrdersModule
{
    public partial class PurchaseOrdersReceivingDialog
    {
        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public int State { get; set; }
        [Parameter]
        public PurchaseOrders Model { get; set; }
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
        private List<Providers> providers = new();
        private List<Projects> projects = new();
        private List<Requisitions> requisitions = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (0 != Model.Id) //representing a Delete dialog
            {
                Title = $"Marcar orden: {Model.Id} como entregada";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = true;
                ButtonColor = Color.Error;
                Model.ReceptionDate = DateTime.Now;
                Model.Status = "Entregada";
            }
            else //should not get to this option
            {
                Dialog.Cancel();
            }
        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            List<ActionResponse<ArticlesToPurchase>> POArticlesResponses = new();
            if (Form.IsValid)
            {
                int SuccessfulArticleResponse = 0;
                var response = await _ordersService.UpdateAsync(Model);
                if (response.IsSuccessful)
                {
                    foreach (var item in Model.ArticlesToPurchase)
                    {
                        item.EntryDate = DateTime.Now;
                        var ArticleResponse = await _articlesService.UpdateAsync(item);
                        POArticlesResponses.Add(ArticleResponse);
                        if (ArticleResponse.IsSuccessful) SuccessfulArticleResponse++;
                    }
                }
                var result = new
                {
                    SuccessOrderUpdate = response.IsSuccessful,
                    SuccessfulArticlesUpdate = SuccessfulArticleResponse,
                    OrderUpdateResponse = response,
                    ArticlesResponses = POArticlesResponses
                };
                Dialog.Close(DialogResult.Ok(JsonConvert.SerializeObject(result)));
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }
    }
}
