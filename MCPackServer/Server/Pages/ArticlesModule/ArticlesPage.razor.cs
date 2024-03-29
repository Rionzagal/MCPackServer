﻿using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using MudBlazor;
using Newtonsoft.Json;

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
        DialogParameters Parameters = new();
        #endregion
        #region MudBlazor Tables
        private MudTable<ArticlesView> ArticleTable = new();
        private MudTable<QuotesView> QuoteTable = new();
        #endregion
        #region Tabs and properties
        private MudTabs ArticleInformationTabs = new();
        private int? _selectedQuoteId = null;
        #endregion
        #region Expansion panels
        private MudExpansionPanel ArticlesPanel = new();
        private MudExpansionPanel FamiliesPanel = new();
        private MudExpansionPanel GroupsPanel = new();
        #endregion
        #region Mud autocomplete components
        private MudAutocomplete<ArticleGroups> GroupsAutocomplete = new();
        private MudAutocomplete<ArticleFamilies> FamiliesAutocomplete = new();
        private MudAutocomplete<int> ProvidersFilterAutocomplete = new();
        private MudAutocomplete<int> QuoteProvidersAutocomplete = new();
        #endregion
        #endregion

        #region API elements
        #region Datamanager Requests
        private DataManagerRequest ArticlesDm = new(), FamiliesDm = new(), GroupsDm = new(), QuotesDm = new();
        #endregion
        #region Search Filters
        private string NameFilter = string.Empty, TradeMarkFilter = string.Empty, CodeFilter = string.Empty;
        private string CurrencyFilter = string.Empty;
        private int ProviderFilter;
        #endregion
        #endregion

        #region Entities and Models
        #region Purchase Articles
        ArticlesView SelectedArticle = new();
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
        List<QuotesView> SelectedQuotes = new();
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
            if (0 == SelectedFamily.Id)
                VisibleArticleInformation = false;
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = new List<WhereFilter>()
                {
                    new WhereFilter
                    {
                        Field = "Name",
                        Value = NameFilter,
                        Condition = Conditions.And,
                        Operator = Operators.Contains
                    },
                    new WhereFilter
                    {
                        Field = "TradeMark",
                        Value = TradeMarkFilter,
                        Operator = Operators.Contains,
                        Condition = Conditions.And
                    },
                    new WhereFilter
                    {
                        Field = "Code",
                        Value = CodeFilter,
                        Operator = Operators.Contains,
                        Condition = Conditions.And
                    },
                    new WhereFilter
                    {
                        Field = "FamilyId",
                        Value = SelectedFamily.Id,
                        Operator = Operators.Equal,
                        Condition = Conditions.And
                    }
                }
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            return new TableData<ArticlesView>
            {
                Items = await _service.GetForGridAsync<ArticlesView>(request, field, order),
                TotalItems = await _service.GetTotalCountAsync<ArticlesView>(request) ?? 0
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
                await ArticleTable.ReloadServerData();
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
                await ArticleTable.ReloadServerData();
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
                VisibleArticleInformation = false;
                SelectedArticle = new();
                ArticlesPanel.Expand();
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
            DataManagerRequest request = new()
            {
                Where = new List<WhereFilter>()
                {
                    new WhereFilter
                    {
                        Field = "Name",
                        Value = filter,
                        Operator = Operators.Contains
                    }
                }
            };
            var items = await _service.GetForGridAsync<ArticleGroups>(request);
            if (null != items)
                GroupsList = items.ToList();
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
                await GroupsServerReload(string.Empty);
                SelectedGroup = GroupsList.OrderByDescending(g => g.Id).First();
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
                await GroupsServerReload(string.Empty);
                SelectedGroup = GroupsList.Single(g => SelectedGroup.Id == g.Id);
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
                SelectedGroup = new();
                await GroupsServerReload(string.Empty);
                SelectedArticle = new();
                SelectedFamily = new();
                VisibleArticleInformation = false;
                ArticlesPanel.Collapse();
                FamiliesPanel.Collapse();
            }
        }
        #endregion

        #region Article Families related methods
        private async Task<IEnumerable<ArticleFamilies>> FamiliesServerReload(string filter)
        {
            SelectedArticle = new();
            ArticlesPanel.Collapse();
            DataManagerRequest request = new()
            {
                Where = new List<WhereFilter>()
                {
                    new WhereFilter
                    {
                        Field = "Name",
                        Value = filter,
                        Operator = Operators.Contains,
                        Condition = Conditions.And
                    },
                    new WhereFilter
                    {
                        Field = "GroupId",
                        Value = SelectedGroup.Id,
                        Operator = Operators.Equal,
                        Condition = Conditions.And
                    }
                }
            };
            var items = await _service.GetForGridAsync<ArticleFamilies>(request);
            if (null != items)
                FamiliesList = items.ToList();
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
                await FamiliesServerReload(string.Empty);
                SelectedFamily = FamiliesList.OrderByDescending(f => f.Id).First();
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
                await FamiliesServerReload(string.Empty);
                SelectedFamily = FamiliesList.Single(f => f.Id == SelectedFamily.Id);
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
                VisibleArticleInformation = false;
                ArticlesPanel.Collapse();
                SelectedArticle = new();
                SelectedFamily = new();
                await FamiliesServerReload(string.Empty);
            }
        }
        #endregion

        #region Quotes related methods
        #region QuoteTable related methods
        private async Task<TableData<QuotesView>> QuotesServerReload(TableState state)
        {
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = new List<WhereFilter>()
                {
                    new WhereFilter
                    {
                        Field = "ArticleId",
                        Value = SelectedArticle.Id,
                        Operator = Operators.Equal
                    },
                    new WhereFilter
                    {
                        Field = "Currency",
                        Value = CurrencyFilter,
                        Operator = Operators.Equal
                    },
                    new WhereFilter
                    {
                        Field = "ProviderId",
                        Value = 0 != ProviderFilter ? ProviderFilter : null,
                        Operator = Operators.Equal
                    },
                }
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<QuotesView>(request, field, order);
            int? count = await _service.GetTotalCountAsync<QuotesView>(request);
            return new TableData<QuotesView>
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedQuoteRow(TableRowClickEventArgs<QuotesView> args)
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
                ["ArticleId"] = SelectedArticle.Id
            };
            var dialog = Dialogs.Show<QuotesDialog>("Añadir nueva cotización", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
                await QuoteTable.ReloadServerData();
        }
        private async Task EditQuote(QuotesView quote)
        {
            Parameters = new()
            {
                ["State"] = QuotesDialog.States.Edit,
                ["ModelView"] = quote,
            };
            var dialog = Dialogs.Show<QuotesDialog>("Editar cotización seleccionada", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                RemoveTab(selectedQuote: quote);
                await QuoteTable.ReloadServerData();
            }
        }
        private async Task DeleteQuote(QuotesView quote)
        {
            Parameters = new()
            {
                ["State"] = QuotesDialog.States.Delete,
                ["ModelView"] = quote
            };
            var dialog = Dialogs.Show<QuotesDialog>("Eliminar cotización seleccionada", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                RemoveTab(selectedQuote: quote);
                await QuoteTable.ReloadServerData();
            }
        }
        private async Task FilterQuotes() => await QuoteTable.ReloadServerData();
        private void DeleteQuoteFilters()
        {
            CurrencyFilter = string.Empty;
            ProviderFilter = 0;
        }
        private void RemoveTab(MudTabPanel? tabPanel = null, QuotesView? selectedQuote = null)
        {
            int? Id = (null != tabPanel) ? (int)tabPanel.Tag : null;
            var quote = selectedQuote ?? SelectedQuotes.FirstOrDefault(q => Id == q.Id);
            if (null != quote) SelectedQuotes.Remove(quote);
        }
        #endregion

        private async Task<IEnumerable<int>> ProvidersServerReload(string filter, bool NewQuote)
        {
            List<int> result = new();
            DataManagerRequest request = new()
            {
                Where = new List<WhereFilter>()
                {
                    new WhereFilter
                    {
                        Field = "MarketName",
                        Value = filter,
                        Operator = Operators.Contains
                    }
                }
            };
            var items = NewQuote ? await _providersService.GetForGridAsync<Providers>(request)
                : await _providersService.GetByQuote(SelectedArticle.Id, request);
            if (null != items)
            {
                ProvidersList = items.ToList();
                result = ProvidersList.Select(p => p.Id).ToList();
            }
            return result;
        }

        private static string GroupName(ArticleGroups input)
        {
            string result = string.Empty;
            if (null != input)
                result = input.Name;
            return result;
        }

        private static string FamilyName(ArticleFamilies input)
        {
            string result = string.Empty;
            if (null != input)
                result = input.Name;
            return result;
        }

        private string GetProviderName(int Id)
        {
            string providerName = string.Empty;
            if (0 != Id)
            {
                var match = ProvidersList.SingleOrDefault(p => Id == p.Id);
                if (null != match)
                    providerName = match.MarketName;
            }
            return providerName;
        }
    }
}
