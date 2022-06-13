using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json.Linq;
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
        private void OnSelectedPurchaseOrder(TableRowClickEventArgs<PurchaseOrdersView> args)
        {
            SelectedOrder = args.Item;
            VisibleOrderInformation = true;
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
                await OrdersTable.ReloadServerData();
                SelectedOrder = await _service.GetByKeyAsync<PurchaseOrdersView>(SelectedOrder.Id);
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
                new WhereFilter { Field = nameof(ArticlesToPurchaseView.PurchaseOrderId), Value = SelectedOrder.Id != 0 ? SelectedOrder.Id.ToString() : string.Empty }
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
            subtotal = 0;
            discount = subtotal * (SelectedOrder.Discount / 100);
            tax = SelectedOrder.HasTaxes ? (subtotal - discount) * 0.16f : 0f;
            total = subtotal + tax - discount;
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
                ["Model"] = article
            };
            var dialog = Dialogs.Show<ArticlesToPurchaseDialog>("Actualizar artículo de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                JObject element = JObject.Parse(result.Data.ToString());
                if (bool.Parse(element.GetValue("Success").ToString()))
                {
                    Snackbar.Add("Artículo de compra actualizado con éxito.", Severity.Success);
                }
                else Snackbar.Add("Error al actualizar artículo seleccionado.", Severity.Error);
            }
        }
        private async Task DeleteArticle(ArticlesToPurchaseView article)
        {
            Parameters = new()
            {
                ["State"] = ArticlesToPurchaseDialog.States.Delete,
                ["Model"] = article
            };
            var dialog = Dialogs.Show<ArticlesToPurchaseDialog>("Eliminar artículo de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                JObject element = JObject.Parse(result.Data.ToString());
                if (bool.Parse(element.GetValue("Success").ToString()))
                {
                    Snackbar.Add("Artículo de compra eliminado con éxito.", Severity.Info);
                }
                else Snackbar.Add("Error al eliminar artículo seleccionado.", Severity.Error);
            }
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
    }
}
