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
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public int State { get; set; }
        [Parameter]
        public int? Id { get; set; }
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
        private PurchaseOrders Model = new();
        private PurchaseOrdersView ModelView = new();
        private HashSet<ArticlesToPurchaseView> SelectedArticles = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (Id.HasValue)
            {
                Model = await _service.GetByKeyAsync<PurchaseOrders>(Id.Value);
                ModelView = await _service.GetByKeyAsync<PurchaseOrdersView>(Id.Value);
            }
            else
                Dialog?.Cancel();

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
                Dialog?.Cancel();
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
                var response = await _service.UpdateAsync(Model);
                if (response.IsSuccessful)
                {
                    foreach (var item in Model.ArticlesToPurchase)
                    {
                        item.EntryDate = DateTime.Now;
                        var ArticleResponse = await _service.UpdateAsync(item);
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
                Dialog?.Close();
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }

        private async Task<TableData<ArticlesToPurchaseView>> ArticlesServerReload(TableState state)
        {
            List<WhereFilter> filters = new()
            {
                new WhereFilter{ Field = nameof(ArticlesToPurchase.PurchaseOrderId), Value = Id?.ToString() ?? string.Empty }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.Page * state.PageSize,
                Where = filters
            };
            string field = state.SortLabel ?? nameof(ArticlesToPurchase.QuoteId);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = (await _service.GetForGridAsync<ArticlesToPurchaseView>(request, field, order)).ToList()
                ?? new List<ArticlesToPurchaseView>();
            int? count = await _service.GetTotalCountAsync<ArticlesToPurchaseView>(request, nameof(ArticlesToPurchaseView.QuoteId));
            if (items.Any())
                items.RemoveAll(i => i.EntryDate.HasValue);
            return new TableData<ArticlesToPurchaseView>()
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
    }
}
