using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;

namespace MCPackServer.Pages.PurchaseOrdersModule
{
    public partial class AddArticlesDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public PurchaseOrders Reference { get; set; }
        #endregion

        private class ArticleModels : ArticlesToPurchase
        {
            private readonly ISnackbar _snackbar;
            private readonly IQuotesService _quotesService;

            public ArticleModels(Quotes quote, ArticleFamilies family, ISnackbar snackbar, IQuotesService quotesService)
            {
                _snackbar = snackbar;
                _quotesService = quotesService;
                Family = family;
                Quote = quote;
                QuoteId = quote.Id;
                SalePrice = quote.Price;
                Quote.Article.Code = $"{family.Group.Code}-{family.Code}-{quote.Article.Code}";
                MustUpdateQuote = (1 == family.Group.HasVariablePrice) & DateTime.Today != quote.DateUpdated.Date;
            }
            public ArticleFamilies Family { get; set; }
            public bool MustUpdateQuote { get; private set; }
            public bool UpdateQuote { get; set; }
            public bool QuoteUpdated { get; private set; }
            public async Task ServerUpdateQuote()
            {
                if (!QuoteUpdated)
                {
                    Quote.Price = SalePrice;
                    Quote.DateUpdated = DateTime.Now;
                    var response = await _quotesService.UpdateAsync(Quote);
                    if (response.IsSuccessful)
                    {
                        _snackbar.Add("Cotización actualizada con éxito", Severity.Success);
                        QuoteUpdated = true;
                        UpdateQuote = false;
                        MustUpdateQuote = false;
                    }
                    else
                        _snackbar.Add("Error al actualizar cotización", Severity.Error);
                }
            }
        }

        #region Dialog variables
        private string Title;
        private string TitleIcon;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private List<ArticleModels> articles = new();
        private HashSet<ArticlesToPurchase> OrderArticles = new();
        private HashSet<ArticleModels> SelectedArticles = new();
        private List<ArticleFamilies> Families = new();
        #endregion

        private MudTable<ArticleModels> ArticlesTable;

        protected override async Task OnInitializedAsync()
        {
            Title = "Añadir nuevo artículo a orden de compra";
            TitleIcon = Icons.Material.Filled.Create;
            Disabled = false;
            ButtonColor = Color.Success;
            OrderArticles = Reference.ArticlesToPurchase.ToHashSet();

            #region Get groups and families
            DataManagerRequest familiesRequest = new();
            var familiesResponse = await _familiesService.GetForGridAsync<ArticleFamilies>(familiesRequest);
            if (null != familiesResponse) Families = familiesResponse.ToList();
            #endregion
        }

        private async Task Submit()
        {
            _processing = true;
            List<ActionResponse<ArticlesToPurchase>> element = new();
            foreach (var Model in SelectedArticles)
            {
                ArticlesToPurchase payload = new()
                {
                    PurchaseOrderId = Reference.Id,
                    QuoteId = Model.QuoteId,
                    Quantity = Model.Quantity,
                    SalePrice = Model.Quote.Price
                };
                var response = await _articlesService.AddAsync(payload);
                if (response.IsSuccessful) element.Add(response);
            }
            Dialog.Close(DialogResult.Ok(JsonSerializer.Serialize(element)));
        }

        private async Task<TableData<ArticleModels>> ArticlesServerReload(TableState state)
        {
            List<Quotes> quotes = new();
            DataManagerRequest dm = new()
            {
                Take = state.PageSize,
                Skip = state.Page * state.PageSize,
                Where = new List<WhereFilter>()
                {
                    new WhereFilter { Field = "ProviderId", Value = Reference.ProviderId.ToString() },
                    new WhereFilter { Field = "Currency", Value = Reference.Currency }
                }
            };
            var items = await _quotesService.GetForGridAsync<Quotes>(dm, "DateUpdated", "DESC");
            int? count = await _quotesService.GetTotalCountAsync<Quotes>(dm);
            if (null != items) quotes = items.ToList();
            foreach (var article in OrderArticles)
            {
                quotes.Remove(quotes.Single(q => article.QuoteId == q.Id));
            }
            foreach (var item in quotes)
            {
                if (!articles.Any(a => item.Id == a.QuoteId))
                {
                    articles.Add(new ArticleModels
                        (quote: item, snackbar: Snackbar,
                        family: Families.Single(f => item.Article.FamilyId == f.Id),
                        quotesService: _quotesService));
                }
            }
            return new TableData<ArticleModels>
            {
                Items = articles,
                TotalItems = count ?? 0
            };
        }
    }
}
