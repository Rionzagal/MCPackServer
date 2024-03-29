﻿using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Linq;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Text.Json;

namespace MCPackServer.Pages.PurchaseOrdersModule
{
    public partial class PurchaseOrdersPage
    {
        #region Dependency injection
        [Inject]
        public NavigationManager? _navigationManager { get; set; }
        [Inject]
        public IJSRuntime? _jsRuntime { get; set; }
        #endregion

        #region Permissions and Flags
        #region Permissions
        private bool CanCreate = false;
        private bool CanEdit = false;
        private bool CanDelete = false;
        #endregion
        #region Visible Flags
        private bool VisibleOrderInformation;
        #endregion
        #endregion

        #region MudBlazor Components
        #region Dialogs
        DialogParameters Parameters = new();
        #endregion
        #region MudBlazor Tables
        private MudTable<PurchaseOrdersView> OrdersTable = new();
        private MudTable<ArticlesToPurchaseView> ArticlesTable = new();
        #endregion
        #region Tabs and properties
        private MudTabs OrderInformationTabs = new();
        private int? _selectedId;
        #endregion
        #endregion

        #region API elements
        #region Search Filters
        private string OrderNumberFilter = string.Empty;
        private string StatusFilter = string.Empty;
        private string ProviderIdFilter = string.Empty;
        private string ProjectIdFilter = string.Empty;
        private string IssuedDateFilter = string.Empty;
        #endregion

        #region Autocomplete filters
        private List<Providers> ProvidersList = new();
        private List<Projects> ProjectsList = new();
        #endregion
        #endregion

        #region Entities and Models
        PurchaseOrdersView SelectedOrder = new();
        List<ArticlesToPurchaseView> SelectedArticles = new();
        private double subtotal, tax, discount, total = 0f;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            var _authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = _authenticationState.User;
            if (null != user)
            {
                try
                {
                    CanCreate = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.PurchaseOrders.Create)).Succeeded;
                    CanEdit = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.PurchaseOrders.Edit)).Succeeded;
                    CanDelete = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.PurchaseOrders.Delete)).Succeeded;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (_selectedId.HasValue)
            {
                OrderInformationTabs.ActivatePanel(_selectedId);
                _selectedId = null;
                StateHasChanged();
            }
        }
        #region PurchaseOrders
        #region OrdersTable methods
        private async Task<TableData<PurchaseOrdersView>> PurchaseOrdersServerReload(TableState state)
        {
            List<WhereFilter> filters = new()
            { 
                new WhereFilter
                {
                    Field = "OrderNumber",
                    Value = OrderNumberFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                },
                new WhereFilter
                {
                    Field = nameof(PurchaseOrders.ProjectId),
                    Value = ProjectIdFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                },
                new WhereFilter
                {
                    Field = nameof(PurchaseOrders.ProviderId),
                    Value = ProviderIdFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                },
                new WhereFilter
                {
                    Field = nameof(PurchaseOrders.IssuedDate),
                    Value = IssuedDateFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                },
                new WhereFilter
                {
                    Field = "Status",
                    Value = StatusFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                }   
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters
            };
            string field = state.SortLabel ?? "IssuedDate";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<PurchaseOrdersView>(request, field, order);
            int? count = await _service.GetTotalCountAsync<PurchaseOrdersView>(request);
            return new TableData<PurchaseOrdersView>
            {
                Items = items ?? new List<PurchaseOrdersView>(),
                TotalItems = count ?? 0
            };
        }
        private async void OnSelectedPurchaseOrder(TableRowClickEventArgs<PurchaseOrdersView> args)
        {
            subtotal = discount = tax = total = 0;
            SelectedOrder = args.Item;
            VisibleOrderInformation = true;
            // Check the price values 
            DataManagerRequest request = new()
            {
                Skip = 0,
                Where = new List<WhereFilter>()
                {
                    new WhereFilter
                    {
                        Field = nameof(ArticlesToPurchaseView.PurchaseOrderId),
                        Value = SelectedOrder.Id != 0 ? SelectedOrder.Id : null,
                        Operator = Operators.Equal
                    }
                }
            };
            var items = await _service.GetForGridAsync<ArticlesToPurchaseView>(request, "Quantity", "ASC", getAll: true);
            if (null != items)
            {
                foreach (var item in items)
                {
                    subtotal += item.SalePrice * item.Quantity;
                }
                discount = subtotal * (SelectedOrder.Discount / 100);
                tax = SelectedOrder.HasTaxes ? (subtotal - discount) * 0.16f : 0f;
                total = subtotal + tax - discount;
            }
        }
        #endregion
        #region CRUD Methods
        private async Task AddOrder()
        {
            Parameters = new()
            {
                ["State"] = PurchaseOrdersDialog.States.Add
            };
            var dialog = Dialogs.Show<PurchaseOrdersDialog>("Añadir nueva órden de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await OrdersTable.ReloadServerData();
                DataManagerRequest dm = new() { Take = 1 };
                SelectedOrder = (await _service.GetForGridAsync<PurchaseOrdersView>(dm, order: "DESC")).First();
                VisibleOrderInformation = true;
            }
        }
        private async Task EditOrder()
        {
            Parameters = new()
            {
                ["State"] = PurchaseOrdersDialog.States.Edit,
                ["ModelView"] = SelectedOrder
            };
            var dialog = Dialogs.Show<PurchaseOrdersDialog>("Actualizar órden de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await OrdersTable.ReloadServerData();
                SelectedOrder = await _service.GetByKeyAsync<PurchaseOrdersView>(SelectedOrder.Id);
            }
        }
        private async Task DeleteOrder()
        {
            Parameters = new()
            {
                ["State"] = PurchaseOrdersDialog.States.Delete,
                ["ModelView"] = SelectedOrder
            };
            var dialog = Dialogs.Show<PurchaseOrdersDialog>("Eliminar órden de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                VisibleOrderInformation = false;
                SelectedOrder = new();
                await OrdersTable.ReloadServerData();
            }
        }
        private async Task MarkOrderAsReceived(int? orderId)
        {
            Parameters = new()
            {
                ["Id"] = orderId
            };
            var dialog = Dialogs.Show<PurchaseOrdersReceivingDialog>("Marcar como recibido", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                VisibleOrderInformation = false;
                SelectedOrder = new();
                await OrdersTable.ReloadServerData();
                if (orderId.HasValue)
                    SelectedOrder = await _service.GetByKeyAsync<PurchaseOrdersView>(orderId);
                VisibleOrderInformation = true;
            }
        }
        #endregion
        #endregion

        #region PurchaseArticles
        #region ArticlesTable methods
        private async Task<TableData<ArticlesToPurchaseView>> ArticlesServerReload(TableState state)
        {
            List<WhereFilter> filters = new()
            {
                new WhereFilter
                {
                    Field = nameof(ArticlesToPurchaseView.PurchaseOrderId),
                    Value = SelectedOrder.Id != 0 ? SelectedOrder.Id : null,
                    Operator = Operators.Equal
                }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.Page * state.PageSize,
                Where = filters,
            };
            string field = state.SortLabel ?? nameof(ArticlesToPurchaseView.Quantity);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<ArticlesToPurchaseView>(request, field, order);
            int? count = await _service.GetTotalCountAsync<ArticlesToPurchaseView>(request, nameof(ArticlesToPurchaseView.QuoteId));
            return new TableData<ArticlesToPurchaseView>
            {
                Items = items ?? new List<ArticlesToPurchaseView>(),
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedArticle(TableRowClickEventArgs<ArticlesToPurchaseView> args)
        {
            var selectedId = args.Item.QuoteId;
            if (!SelectedArticles.Any(a => selectedId == a.QuoteId))
            {
                SelectedArticles.Add(args.Item);
                _selectedId = selectedId;
            }
        }
        #endregion
        #region CRUD methods
        private async Task AddArticles()
        {
            Parameters = new()
            {
                ["State"] = AddArticlesDialog.States.Add,
                ["OrderId"] = SelectedOrder.Id
            };
            var dialog = Dialogs.Show<AddArticlesDialog>("Añadir artículos a orden", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ArticlesTable.ReloadServerData();
            }
        }
        private async Task EditArticle(ArticlesToPurchaseView article)
        {
            Parameters = new()
            {
                ["State"] = ArticlesToPurchaseDialog.States.Edit,
                ["ModelView"] = article
            };
            var dialog = Dialogs.Show<ArticlesToPurchaseDialog>("Actualizar artículo de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                RemoveTab(article: article);
            }
        }
        private async Task DeleteArticle(ArticlesToPurchaseView article)
        {
            Parameters = new()
            {
                ["State"] = ArticlesToPurchaseDialog.States.Delete,
                ["ModelView"] = article
            };
            var dialog = Dialogs.Show<ArticlesToPurchaseDialog>("Eliminar artículo de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
                RemoveTab(article: article);
        }
        #endregion
        #endregion

        private void GoToReport()
        {
            _navigationManager?.NavigateTo($"/Report/{SelectedOrder.Id}");
        }

        private void RemoveTab(MudTabPanel? tabPanel = null, ArticlesToPurchaseView? article = null)
        {
            int? id = (null != tabPanel) ? (int)tabPanel.Tag : null;
            var selectedArticle = article ?? SelectedArticles.FirstOrDefault(p => id == p.QuoteId);
            if (null != selectedArticle) SelectedArticles.Remove(selectedArticle);
        }

        private async Task<List<string>> ProvidersServerReload(string filter)
        {
            List<string> result = new();
            DataManagerRequest dm = new()
            {
                Where = new()
                {
                    new WhereFilter { Field = nameof(Providers.LegalName), Operator = Operators.Contains, Value = filter }
                }
            };
            var items = await _service.GetForGridAsync<Providers>(dm);
            if (null != items)
            {
                ProvidersList = items.ToList();
                ProvidersList.ForEach(p => result.Add(p.Id.ToString()));
            }
            return result;
        }

        private async Task<List<string>> ProjectsServerReload(string filter)
        {
            List<string> result = new();
            DataManagerRequest dm = new()
            {
                Where = new()
                {
                    new WhereFilter
                    { Field = nameof(Projects.ProjectNumber), Operator = Operators.Contains, Value=filter }
                }
            };
            var items = await _service.GetForGridAsync<Projects>(dm);
            if (null != items)
            {
                ProjectsList = items.ToList();
                ProjectsList.ForEach(p => result.Add(p.Id.ToString()));
            }
            return result;
        }

        private string GetProviderLegalName(string Id)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(Id)) 
            {
                var match = ProvidersList.FirstOrDefault(p => Id == p.Id.ToString());
                if (null != match) result = match.LegalName;
            }
            return result;
        }

        private string GetProjectNumber(string Id)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(Id))
            {
                var match = ProjectsList.FirstOrDefault(p => Id == p.Id.ToString());
                if (null != match) result = match.ProjectNumber;
            }
            return result;
        }
    }
}
