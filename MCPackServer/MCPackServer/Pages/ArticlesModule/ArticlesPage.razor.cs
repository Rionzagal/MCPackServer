using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using MudBlazor;

namespace MCPackServer.Pages.ArticlesModule
{
    public partial class ArticlesPage
    {
        #region Dependency Injection
        #endregion

        #region Permissions and Flags
        #region Permissions
        private bool CanCreateArticle, CanEditArticle, CanDeleteArticle = false;
        private bool CanCreateFamily, CanEditFamily, CanDeleteFamily = false;
        private bool CanCreateGroup, CanEditGroup, CanDeleteGroup = false;
        private bool CanCreateQuote, CanViewQuote, CanEditQuote, CanDeleteQuote = false;
        #endregion
        #region Visible Flags
        private bool VisibleArticleInformation;
        #endregion
        #endregion

        #region MudBlazor Components
        #region Dialogs
        DialogParameters Parameters;
        #endregion
        #region MudBlazor Tables
        private MudTable<ArticlesView> ArticleTable = new();
        private MudTable<Quotes> QuoteTable = new();
        #endregion
        #region Tabs and properties
        private MudTabs ArticleInformationTabs;
        private int? _selectedQuoteId = null;
        #endregion
        #region Expansion panels
        private MudExpansionPanel ArticlesPanel;
        private MudExpansionPanel FamiliesPanel;
        private MudExpansionPanel GroupsPanel;
        #endregion
        #region Mud autocomplete components
        private MudAutocomplete<ArticleGroups> GroupsAutocomplete;
        private MudAutocomplete<ArticleFamilies> FamiliesAutocomplete;
        private MudAutocomplete<int> ProvidersFilterAutocomplete;
        private MudAutocomplete<int> QuoteProvidersAutocomplete;
        #endregion
        #endregion

        #region API elements
        #region Datamanager Requests
        private DataManagerRequest ArticlesDm, FamiliesDm, GroupsDm, QuotesDm;
        #endregion
        #region Search Filters
        private string NameFilter, TradeMarkFilter, CodeFilter;
        private string CurrencyFilter;
        private int ProviderFilter;
        #endregion
        #endregion

        #region Entities and Models
        #region Purchase Articles
        ArticlesView SelectedArticle = new();
        private string ArticleCode = string.Empty;
        #endregion
        #region Article Groups
        ArticleGroups SelectedGroup = new();
        List<ArticleGroups> GroupsList = new();
        #endregion
        #region Article Families
        ArticleFamilies SelectedFamily = new();
        List<ArticleFamilies> FamiliesList = new();
        #endregion
        #region Quotes
        List<Quotes> SelectedQuotes = new();
        #endregion
        List<Providers> ProvidersList = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            var _authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = _authenticationState.User;
            if (user != null)
            {
                try
                {
                    CanCreateArticle = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Articles.Create)).Succeeded;
                    CanEditArticle = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Articles.Edit)).Succeeded;
                    CanDeleteArticle = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Articles.Delete)).Succeeded;

                    CanCreateFamily = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.ArticleFamilies.Create)).Succeeded;
                    CanEditFamily = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.ArticleFamilies.Edit)).Succeeded;
                    CanDeleteFamily = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.ArticleFamilies.Delete)).Succeeded;

                    CanCreateGroup = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.ArticleGroups.Create)).Succeeded;
                    CanEditGroup = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.ArticleGroups.Edit)).Succeeded;
                    CanDeleteGroup = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.ArticleGroups.Delete)).Succeeded;

                    CanViewQuote = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Quotes.View)).Succeeded;
                    CanCreateQuote = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Quotes.Create)).Succeeded;
                    CanEditQuote = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Quotes.Edit)).Succeeded;
                    CanDeleteQuote = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Quotes.Delete)).Succeeded;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (_selectedQuoteId.HasValue)
            {
                ArticleInformationTabs.ActivatePanel(_selectedQuoteId);
                _selectedQuoteId = null;
                StateHasChanged();
            }
        }

        #region Purchase Articles related methods
        #region ArticleTable methods
        private async Task<TableData<ArticlesView>> ArticleServerReload(TableState state)
        {
            if (0 == SelectedFamily.Id) VisibleArticleInformation = false;
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "Name", Value = NameFilter },
                new WhereFilter { Field = "TradeMark", Value = TradeMarkFilter },
                new WhereFilter { Field = "Code", Value = CodeFilter },
                new WhereFilter { Field = "FamilyId", Value = SelectedFamily.Id.ToString() }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<ArticlesView>(request, field, order);
            int? count = await _service.GetTotalCountAsync<ArticlesView>(request);
            return new TableData<ArticlesView>
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedArticleRow(TableRowClickEventArgs<ArticlesView> args)
        {
            SelectedArticle = args.Item;
            GroupsPanel.Collapse();
            FamiliesPanel.Collapse();
            ArticlesPanel.Collapse();
            VisibleArticleInformation = true;
        }
        private void DeleteArticleSearchFilters() =>
            NameFilter = TradeMarkFilter = CodeFilter = string.Empty;
        private async Task FilterArticles() => await ArticleTable.ReloadServerData();
        #endregion
        private async Task CreateArticle()
        {
            Parameters = new()
            {
                ["State"] = ArticlesDialog.States.Add,
                ["FamilyId"] = SelectedFamily.Id
            };
            var dialog = Dialogs.Show<ArticlesDialog>("Añadir nuevo artículo", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<PurchaseArticles> response = (ActionResponse<PurchaseArticles>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Artículo añadido con éxito.", Severity.Success);
                }
                else
                {
                    foreach (var error in response.Errors)
                    {
                        Snackbar.Add(error, Severity.Error);
                    }
                }
                await ArticleTable.ReloadServerData();
            }
        }
        private async Task EditArticle()
        {
            Parameters = new()
            {
                ["State"] = ArticlesDialog.States.Edit,
                ["ModelView"] = SelectedArticle
            };
            var dialog = Dialogs.Show<ArticlesDialog>("Editar artículo seleccionado", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<PurchaseArticles> response = (ActionResponse<PurchaseArticles>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Artículo editado con éxito.", Severity.Success);
                }
                else
                {
                    foreach (var error in response.Errors)
                    {
                        Snackbar.Add(error, Severity.Error);
                    }
                }
                await ArticleTable.ReloadServerData();
            }
        }
        private async Task DeleteArticle()
        {
            Parameters = new()
            {
                ["State"] = ArticlesDialog.States.Delete,
                ["ModelView"] = SelectedArticle
            };
            var dialog = Dialogs.Show<ArticlesDialog>("Eliminar artículo seleccionado", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<PurchaseArticles> response = (ActionResponse<PurchaseArticles>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Artículo eliminado con éxito.", Severity.Info);
                    VisibleArticleInformation = false;
                    SelectedArticle = new();
                }
                else
                {
                    foreach (var error in response.Errors)
                    {
                        Snackbar.Add(error, Severity.Error);
                    }
                }
                await ArticleTable.ReloadServerData();
            }
        }
        private async Task RenderArticles()
        {
            if (ArticlesPanel.IsExpanded && 0 != SelectedFamily.Id)
                await ArticleTable.ReloadServerData();
        }
        #endregion

        #region Article Groups related methods
        private async Task<IEnumerable<ArticleGroups>> GroupsServerReload(string filter)
        {
            SelectedFamily = new();
            FamiliesList = new();
            ArticlesPanel.Collapse();
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "Name", Value = filter }
            };
            DataManagerRequest request = new()
            {
                Where = filters
            };
            var items = await _service.GetForGridAsync<ArticleGroups>(request);
            if (null != items) GroupsList = items.ToList();
            return GroupsList;
        }
        private async Task CreateGroup()
        {
            Parameters = new()
            {
                ["State"] = GroupsDialog.States.Add,
                ["Model"] = new ArticleGroups()
            };
            var dialog = Dialogs.Show<GroupsDialog>("Añadir nuevo grupo", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<ArticleGroups> response = (ActionResponse<ArticleGroups>)result.Data;
                if (response.IsSuccessful)
                    Snackbar.Add("Grupo añadido con éxito.", Severity.Success);
                else
                    Snackbar.Add("Error al añadir grupo.", Severity.Error);
            }
        }
        private async Task EditGroup()
        {
            Parameters = new()
            {
                ["State"] = GroupsDialog.States.Edit,
                ["Model"] = SelectedGroup
            };
            var dialog = Dialogs.Show<GroupsDialog>("Editar nuevo grupo", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<ArticleGroups> response = (ActionResponse<ArticleGroups>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Grupo editado con éxito.", Severity.Success);
                    SelectedGroup = new();
                }
                else
                    Snackbar.Add("Error al editar grupo.", Severity.Error);
            }
        }
        private async Task DeleteGroup()
        {
            Parameters = new()
            {
                ["State"] = GroupsDialog.States.Delete,
                ["Model"] = SelectedGroup
            };
            var dialog = Dialogs.Show<GroupsDialog>("Eliminar grupo", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<ArticleGroups> response = (ActionResponse<ArticleGroups>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Grupo eliminado con éxito.", Severity.Info);
                    GroupsPanel.Collapse();
                    SelectedGroup = new();
                }
                else
                    Snackbar.Add("Error al eliminar grupo.", Severity.Error);
            }
        }
        #endregion

        #region Article Families related methods
        private async Task<IEnumerable<ArticleFamilies>> FamiliesServerReload(string filter)
        {
            SelectedArticle = new();
            ArticlesPanel.Collapse();
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "Name", Value = filter },
                new WhereFilter { Field = "GroupId", Value = SelectedGroup.Id.ToString() }
            };
            DataManagerRequest request = new()
            {
                Where = filters
            };
            var items = await _familiesService.GetForGridAsync<ArticleFamilies>(request);
            if (null != items) FamiliesList = items.ToList();
            return FamiliesList;
        }
        private async Task CreateFamily()
        {
            Parameters = new()
            {
                ["State"] = FamiliesDialog.States.Add,
                ["Model"] = new ArticleFamilies() { GroupId = SelectedGroup.Id }
            };
            var dialog = Dialogs.Show<FamiliesDialog>("Añadir nueva familia", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<ArticleFamilies> response = (ActionResponse<ArticleFamilies>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Familia añadida con éxito.", Severity.Success);
                }
                else
                    Snackbar.Add("Error al añadir familia.", Severity.Error);
            }
        }
        private async Task EditFamily()
        {
            Parameters = new()
            {
                ["State"] = FamiliesDialog.States.Edit,
                ["Model"] = SelectedFamily
            };
            var dialog = Dialogs.Show<FamiliesDialog>("Editar familia seleccionada", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<ArticleFamilies> response = (ActionResponse<ArticleFamilies>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Familia editada con éxito.", Severity.Success);
                    SelectedFamily = new();
                }
                else
                    Snackbar.Add("Error al editar familia.", Severity.Error);
            }
        }
        private async Task DeleteFamily()
        {
            Parameters = new()
            {
                ["State"] = FamiliesDialog.States.Edit,
                ["Model"] = SelectedFamily
            };
            var dialog = Dialogs.Show<FamiliesDialog>("Eliminar familia seleccionada", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<ArticleFamilies> response = (ActionResponse<ArticleFamilies>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Familia eliminada con éxito.", Severity.Success);
                    SelectedGroup = new();
                }
                else
                    Snackbar.Add("Error al eliminar familia.", Severity.Error);
            }
        }
        #endregion

        #region Quotes related methods
        #region QuoteTable related methods
        private async Task<TableData<Quotes>> QuotesServerReload(TableState state)
        {
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "ArticleId", Value = SelectedArticle.Id.ToString() },
                new WhereFilter { Field = "Currency", Value = CurrencyFilter },
                new WhereFilter { Field = "ProviderId", Value = 0 != ProviderFilter ? ProviderFilter.ToString() : string.Empty },
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _quotesService.GetForGridAsync<Quotes>(request, field, order);
            int? count = await _quotesService.GetTotalCountAsync<Quotes>(request);
            return new TableData<Quotes>
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedQuoteRow(TableRowClickEventArgs<Quotes> args)
        {
            var selectedId = args.Item.Id;
            if (!SelectedQuotes.Any(q => selectedId == q.Id))
            {
                SelectedQuotes.Add(args.Item);
                _selectedQuoteId = selectedId;
            }
        }
        #endregion
        private async Task CreateQuote()
        {
            Parameters = new()
            {
                ["State"] = QuotesDialog.States.Add,
                ["Model"] = new Quotes() { ArticleId = SelectedArticle.Id },
                ["ArticleId"] = SelectedArticle.Id
            };
            var dialog = Dialogs.Show<QuotesDialog>("Añadir nueva cotización", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Quotes> response = (ActionResponse<Quotes>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Cotización añadida con éxito.", Severity.Success);
                }
                else
                    Snackbar.Add("Error al añadir cotización.", Severity.Error);
            }
            await QuoteTable.ReloadServerData();
        }
        private async Task EditQuote(Quotes quote)
        {
            Parameters = new()
            {
                ["State"] = QuotesDialog.States.Edit,
                ["Model"] = quote,
                ["ArticleId"] = SelectedArticle.Id
            };
            var dialog = Dialogs.Show<QuotesDialog>("Editar cotización seleccionada", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Quotes> response = (ActionResponse<Quotes>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Cotización editada con éxito.", Severity.Success);
                    RemoveTab(selectedQuote: quote);
                }
                else
                    Snackbar.Add("Error al editar cotización.", Severity.Error);
            }
            await QuoteTable.ReloadServerData();
        }
        private async Task DeleteQuote(Quotes quote)
        {
            Parameters = new()
            {
                ["State"] = QuotesDialog.States.Delete,
                ["Model"] = quote,
                ["ArticleId"] = SelectedArticle.Id
            };
            var dialog = Dialogs.Show<QuotesDialog>("Eliminar cotización seleccionada", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Quotes> response = (ActionResponse<Quotes>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Cotización eliminada con éxito.", Severity.Info);
                    RemoveTab(selectedQuote: quote);
                }
                else
                    Snackbar.Add("Error al eliminar cotización.", Severity.Error);
            }
            await QuoteTable.ReloadServerData();
        }
        private async Task FilterQuotes() => await QuoteTable.ReloadServerData();
        private void DeleteQuoteFilters()
        {
            CurrencyFilter = string.Empty;
            ProviderFilter = 0;
        }
        private void RemoveTab(MudTabPanel? tabPanel = null, Quotes? selectedQuote = null)
        {
            int? Id = (null != tabPanel) ? (int)tabPanel.Tag : null;
            var quote = selectedQuote ?? SelectedQuotes.FirstOrDefault(q => Id == q.Id);
            if (null != quote) SelectedQuotes.Remove(quote);
        }
        #endregion

        private async Task<IEnumerable<int>> ProvidersServerReload(string filter, bool NewQuote)
        {
            List<int> result = new();
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "MarketName", Value = filter }
            };
            DataManagerRequest request = new()
            {
                Where = filters
            };
            var items = NewQuote ? await _providersService.GetForGridAsync<Providers>(request)
                : await _providersService.GetByQuote(SelectedArticle.Id, request);
            if (null != items)
            {
                ProvidersList = items.ToList();
                ProvidersList.ForEach(p => result.Add(p.Id));
            }
            return result;
        }

        private static string GroupName(ArticleGroups input)
        {
            string result = string.Empty;
            if (null != input) result = input.Name;
            return result;
        }

        private static string FamilyName(ArticleFamilies input)
        {
            string result = string.Empty;
            if (null != input) result = input.Name;
            return result;
        }

        private string GetProviderName(int Id)
        {
            string providerName = string.Empty;
            if (0 != Id)
            {
                var match = ProvidersList.SingleOrDefault(p => Id == p.Id);
                if (null != match) providerName = match.MarketName;
            }
            return providerName;
        }
    }
}
