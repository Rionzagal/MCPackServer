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
        private List<ArticlesToPurchase> ArticleModels = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (Id.HasValue)
            {
                Model = await _service.GetByKeyAsync<PurchaseOrders>(Id.Value);
                ModelView = await _service.GetByKeyAsync<PurchaseOrdersView>(Id.Value);
                DataManagerRequest request = new()
                {
                    Where = new List<WhereFilter>()
                    {
                        new WhereFilter { Field = nameof(ArticlesToPurchase.PurchaseOrderId), Value = Id, Operator = Operators.Equal }
                    }
                };
                ArticleModels = (await _service.GetForGridAsync<ArticlesToPurchase>(
                    request, sortField: nameof(ArticlesToPurchase.QuoteId), getAll: true
                    )).
                    ToList();
                Title = $"Recibir artículos para orden No. {ModelView.OrderNumber}";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = true;
                ButtonColor = Color.Primary;
                Model.ReceptionDate = DateTime.Now;
            }
            else
                Dialog?.Cancel();
        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            List<ActionResponse<ArticlesToPurchase>> SuccessfulArticleResponses = new();
            List<ActionResponse<ArticlesToPurchase>> FailedArticleResponses = new();
            if (Form.IsValid)
            {
                var receptionDate = DateTime.Now;
                foreach (var item in ArticleModels.Where(a => SelectedArticles.Select(x => x.QuoteId).Contains(a.QuoteId)))
                {
                    item.ReceptionDate = receptionDate;
                    var ArticleResponse = await _service.UpdateAsync(item);
                    if (ArticleResponse.IsSuccessful)
                        SuccessfulArticleResponses.Add(ArticleResponse);
                    else
                        FailedArticleResponses.Add(ArticleResponse);
                }
                if (SuccessfulArticleResponses.Any())
                    Snackbar.Add($"{SuccessfulArticleResponses.Count} de {SelectedArticles.Count} " +
                        $"artículos han sido correctamente marcados como recibidos.",
                        Severity.Success);
                if (FailedArticleResponses.Any())
                {
                    Snackbar.Add($"Se han detectado errores en {FailedArticleResponses.Count} de {SelectedArticles.Count} " +
                        $"artículos seleccionados.",
                        Severity.Error);
                    for (int i = 0; i < FailedArticleResponses.Count; i++)
                    {
                        Snackbar.Add($"Error {i + 1} de {FailedArticleResponses.Count}: " +
                            FailedArticleResponses[i].Errors.ToString(), Severity.Error);
                    }
                }
                Model.Status = ArticleModels.Any(x => !x.ReceptionDate.HasValue) ? "RECEPCIÓN PARCIAL" : "RECIBIDA";
                var response = await _service.UpdateAsync(Model);
                if (response.IsSuccessful)
                    Snackbar.Add($"La orden de compra ha sido cambiada a estado de: {Model.Status}", Severity.Success);
                else
                    foreach (var item in response.Errors)
                    {
                        Snackbar.Add(item, Severity.Error);
                    }
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
                new WhereFilter{ Field = nameof(ArticlesToPurchase.PurchaseOrderId), Value = Id, Operator = Operators.Equal }
            };
            DataManagerRequest request = new()
            {
                Skip = 0,
                Where = filters
            };
            string field = state.SortLabel ?? nameof(ArticlesToPurchase.QuoteId);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = (await _service.GetForGridAsync<ArticlesToPurchaseView>(request, field, order, getAll: true)).ToList()
                ?? new List<ArticlesToPurchaseView>();
            int? count = await _service.GetTotalCountAsync<ArticlesToPurchaseView>(request, nameof(ArticlesToPurchaseView.QuoteId));
            if (items.Any())
                items.RemoveAll(i => i.ReceptionDate.HasValue);
            return new TableData<ArticlesToPurchaseView>()
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
    }
}
