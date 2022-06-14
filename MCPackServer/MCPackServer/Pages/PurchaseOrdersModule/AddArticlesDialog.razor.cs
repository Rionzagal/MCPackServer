using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Text.Json;

namespace MCPackServer.Pages.PurchaseOrdersModule
{
    public partial class AddArticlesDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public int? OrderId { get; set; }
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        private class OrderArticles : QuotesView
        {
            private readonly ISnackbar _snackbar;
            private readonly IBaseService _service;

            public OrderArticles(QuotesView quote, ISnackbar snackbar, IBaseService service)
            {
                _snackbar = snackbar;
                _service = service;

                var properties = quote.GetType().GetProperties()
                    .Where(p => p.GetAccessors()[0].IsFinal || !p.GetAccessors()[0].IsVirtual);
                foreach (var item in properties)
                {
                    var value = item.GetValue(quote);
                    item.SetValue(this, value);
                }
                MustUpdateQuote = quote.MustQuoteDaily && (DateTime.Today != quote.DateUpdated.Date);
            }
            public int Quantity { get; set; }
            public bool MustUpdateQuote { get; private set; }
            public bool UpdateFlag { get; set; }
            public bool QuoteUpdated { get; private set; }
            public async Task Update()
            {
                if (!QuoteUpdated)
                {
                    Quotes payload = new()
                    {
                        Id = Id,
                        ArticleId = ArticleId,
                        Price = Price,
                        ProviderId = ProviderId,
                        Currency = Currency,
                        SKU = SKU,
                        DateUpdated = DateTime.Now
                    };
                    var response = await _service.UpdateAsync(payload);
                    if (response.IsSuccessful)
                    {
                        DateUpdated = payload.DateUpdated;
                        UpdateFlag = false;
                        QuoteUpdated = true;
                        MustUpdateQuote = false;
                        _snackbar.Add("Cotización actualizada con éxito.", Severity.Success);
                    }
                    else
                        foreach (var item in response.Errors)
                        {
                            _snackbar.Add(item, Severity.Error);
                        }
                }
            }
        }

        #region Models
        private List<OrderArticles> TableArticles = new();
        private HashSet<OrderArticles> SelectedArticles = new();
        private PurchaseOrdersView Reference = new();
        private Providers OrderProvider = new();
        private List<ArticlesToPurchase> OrderedArticles = new();
        private List<RequisitionArticles> RequestedArticles = new();
        #endregion

        private MudTable<OrderArticles> ArticlesTable = new();
        

        protected override async Task OnInitializedAsync()
        {
            if (!OrderId.HasValue)
                Dialog?.Cancel();
            else
            {
                Reference = await _service.GetByKeyAsync<PurchaseOrdersView>(OrderId, nameof(PurchaseOrdersView.Id));
                OrderProvider = await _service.GetByKeyAsync<Providers>(Reference.ProviderId, nameof(Providers.Id));
                DataManagerRequest OrderArticlesRequest = new()
                {
                    Where = new()
                    {
                        new WhereFilter { Field = nameof(ArticlesToPurchase.PurchaseOrderId), Value = OrderId?.ToString() ?? "" }
                    }
                };
                OrderedArticles = (await _service.GetForGridAsync<ArticlesToPurchase>(OrderArticlesRequest,
                    sortField: nameof(ArticlesToPurchase.QuoteId), getAll: true)).ToList();
                if (Reference.RequisitionId.HasValue) // Check if the Reference Order is linked to a requisition
                {
                    DataManagerRequest RequisitionArticlesRequest = new()
                    {
                        Where = new()
                        {
                            new WhereFilter
                            {
                                Field = nameof(RequisitionArticles.RequisitionId),
                                Value = Reference.RequisitionId?.ToString() ?? ""
                            },
                            new WhereFilter
                            {
                                Field = nameof(RequisitionArticles.ProjectId),
                                Value = Reference.ProjectId.ToString()
                            }
                        }
                    };
                    RequestedArticles = (await _service.GetForGridAsync<RequisitionArticles>(
                        RequisitionArticlesRequest, nameof(RequisitionArticles.ArticleId), getAll: true))
                        .ToList(); // Recover the requisition articles linked to the project of the Purchase Order
                    foreach (var article in RequestedArticles)
                    {
                        QuotesView? QuotedArticle = (await _service.GetForGridAsync<QuotesView>(
                            new DataManagerRequest()
                            {
                                Take = 1,
                                Where = new List<WhereFilter>()
                                {
                                    new WhereFilter { Field = nameof(QuotesView.ArticleId), Value = article.ArticleId.ToString() },
                                    new WhereFilter { Field = nameof(QuotesView.ProviderId), Value = Reference.ProviderId.ToString() },
                                    new WhereFilter { Field = nameof(QuotesView.Currency), Value = Reference.Currency }
                                }
                            })).FirstOrDefault(); // Search for a quote that matches the requested article and the provider
                        if (QuotedArticle != null) // If it is found, add that quote as an ordered article and select it
                            SelectedArticles.Add(new(QuotedArticle, Snackbar, _service)
                            {
                                Quantity = article.Quantity
                            });
                    }
                }
            }
            Title = "Añadir artículos a orden de compra";
            TitleIcon = Icons.Material.Filled.Create;
            Disabled = false;
            ButtonColor = Color.Success;
        }

        private async Task Submit()
        {
            _processing = true;
            List<ActionResponse<ArticlesToPurchase>> SuccessfulResponses = new();
            List<ActionResponse<ArticlesToPurchase>> FailedResponses = new();
            foreach (var Model in SelectedArticles)
            {
                ArticlesToPurchase payload = new()
                {
                    PurchaseOrderId = Reference.Id,
                    QuoteId = Model.Id,
                    Quantity = Model.Quantity,
                    SalePrice = Model.Price
                };
                var response = await _service.AddAsync(payload);
                if (response.IsSuccessful)
                    SuccessfulResponses.Add(response);
                else
                    FailedResponses.Add(response);
            }
            if (SuccessfulResponses.Any())
                Snackbar.Add($"{SuccessfulResponses.Count} de {SelectedArticles.Count} artículos seleccionados " +
                    $"añadidos correctamente a la orden de compra: {Reference.OrderNumber}.", Severity.Success);
            if (FailedResponses.Any())
            {
                for (int i = 0; i < FailedResponses.Count; i++)
                {
                    Snackbar.Add($"ERROR {i + 1} / {FailedResponses.Count}: {FailedResponses[i].Errors}", Severity.Error);
                }
            }
            Dialog?.Close();
        }

        private async Task<TableData<OrderArticles>> ArticlesServerReload(TableState state)
        {
            List<OrderArticles> items = new();
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.Page * state.PageSize,
                Where = new List<WhereFilter>()
                {
                    new WhereFilter { Field = "ProviderId", Value = Reference?.ProviderId.ToString() ?? "" },
                    new WhereFilter { Field = "Currency", Value = Reference?.Currency ?? "" }
                }
            };
            string sortField = state.SortLabel ?? nameof(QuotesView.DateUpdated);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var response = (await _service.GetForGridAsync<QuotesView>(request, sortField, order))?
                .ToList() ?? new List<QuotesView>();
            response.ForEach(q => items.Add(new OrderArticles(q, Snackbar, _service)));
            int count = await _service.GetTotalCountAsync<QuotesView>(request) ?? 0;
            if (count > 0)
                count -= items.RemoveAll(q => OrderedArticles.Any(a => q.Id == a.QuoteId));
            TableArticles = items;
            return new TableData<OrderArticles>()
            {
                Items = items,
                TotalItems = count
            };
        }

        private void OnSelectedArticlesChange(HashSet<OrderArticles> articles)
        {
            TableArticles.ForEach(item => item.Quantity = !articles.Any(a => a.Id == item.Id) ? 0 : item.Quantity);
        }

        private void OnCommitRow(object args)
        {
            if (args.GetType().Name == nameof(OrderArticles))
            {
                var article = (OrderArticles)args;
                SelectedArticles.Add(article);
            }
        }
    }
}
