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
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public IJSRuntime _jsRuntime { get; set; }
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
        DialogParameters Parameters;
        #endregion
        #region MudBlazor Tables
        private MudTable<PurchaseOrders> OrdersTable;
        private MudTable<ArticlesToPurchase> ArticlesTable;
        #endregion
        #region Tabs and properties
        private MudTabs OrderInformationTabs;
        private int? _selectedId;
        #endregion
        #endregion

        #region API elements
        #region Search Filters

        #endregion
        #endregion

        #region Entities and Models
        PurchaseOrders SelectedOrder = new();
        List<Clients> ProjectClients = new();
        List<ArticlesToPurchase> SelectedArticles = new();
        List<ArticleGroups> Groups = new();
        List<ArticleFamilies> Families = new();
        private float subtotal, tax, discount, total = 0f;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            try
            {
                DataManagerRequest request = new();
                var clients = await _service.GetForGridAsync<Clients>(request);
                if (null != clients) ProjectClients = clients.ToList();
                var groups = await _service.GetForGridAsync<ArticleGroups>(request);
                if (null != groups) Groups = groups.ToList();
                var families = await _service.GetForGridAsync<ArticleFamilies>(request);
                if (null != families) Families = Families.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
        private async Task<TableData<PurchaseOrders>> PurchaseOrdersServerReload(TableState state)
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
            var items = await _ordersService.GetForGridAsync<PurchaseOrders>(request, field, order);
            int? count = await _ordersService.GetTotalCountAsync<PurchaseOrders>(request);
            return new TableData<PurchaseOrders>
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private async Task OnSelectedPurchaseOrder(TableRowClickEventArgs<PurchaseOrders> args)
        {
            VisibleOrderInformation = false;
            SelectedOrder = args.Item;
            SelectedOrder.ArticlesToPurchase = await ArticlesServerReload(args.Item.Id);
            SelectedOrder.Project.Client = ProjectClients.Single(c => SelectedOrder.Project.ClientId == c.Id);
            VisibleOrderInformation = true;
        }
        #endregion
        #region CRUD Methods
        private async Task AddOrder()
        {
            Parameters = new()
            {
                ["State"] = PurchaseOrdersDialog.States.Add,
                ["Model"] = new PurchaseOrders()
                {
                    IssuedDate = DateTime.Today,
                    DeliveryDate = DateTime.Today
                }
            };
            var dialog = Dialogs.Show<PurchaseOrdersDialog>("Añadir nueva órden de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<PurchaseOrders> response = (ActionResponse<PurchaseOrders>)result.Data;
                if (response.IsSuccessful)
                    Snackbar.Add("Órden de compra generada con éxito.", Severity.Success);
                else
                    Snackbar.Add("Error al generar órden de compra.", Severity.Error);
                await OrdersTable.ReloadServerData();
            }
        }
        private async Task EditOrder()
        {
            Parameters = new()
            {
                ["State"] = PurchaseOrdersDialog.States.Edit,
                ["Model"] = SelectedOrder
            };
            var dialog = Dialogs.Show<PurchaseOrdersDialog>("Actualizar órden de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<PurchaseOrders> response = (ActionResponse<PurchaseOrders>)result.Data;
                if (response.IsSuccessful)
                {
                    VisibleOrderInformation = false;
                    SelectedOrder = new();
                    Snackbar.Add("Órden de compra actualizada con éxito.", Severity.Success);
                }
                else
                    Snackbar.Add("Error al actualizar órden de compra.", Severity.Error);
                await OrdersTable.ReloadServerData();
            }
        }
        private async Task DeleteOrder()
        {
            Parameters = new()
            {
                ["State"] = PurchaseOrdersDialog.States.Delete,
                ["Model"] = SelectedOrder
            };
            var dialog = Dialogs.Show<PurchaseOrdersDialog>("Eliminar órden de compra", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<PurchaseOrders> response = (ActionResponse<PurchaseOrders>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Órden de compra eliminada con éxito.", Severity.Info);
                    VisibleOrderInformation = false;
                    SelectedOrder = new();
                }
                else
                    Snackbar.Add("Error al eliminar órden de compra.", Severity.Error);
                await OrdersTable.ReloadServerData();
            }
        }

        private async Task MarkOrderAsReceived()
        {
            Parameters = new() { ["Model"] = SelectedOrder };
            var dialog = Dialogs.Show<PurchaseOrdersReceivingDialog>("Marcar como recibido", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                JObject element = new();
                var resultString = result.Data.ToString();
                bool Success = false;
                if (!string.IsNullOrEmpty(resultString))
                {
                    element = JObject.Parse(resultString);
                    Success = bool.Parse(element.GetValue("SuccessOrderUpdate").ToString() ?? "false");
                }
                    
                if (Success)
                {
                    Snackbar.Add("Orden de compra marcada exitosamente como recibida", Severity.Success);
                    Snackbar.Add($"{int.Parse(element.GetValue("SuccessfulArticlesUpdate").ToString())} artículos se han recibido", Severity.Success);
                }
                else
                    Snackbar.Add("Error al marcar orden de compra como recibida", Severity.Error);
                await OrdersTable.ReloadServerData();
                SelectedOrder = await _ordersService.GetByKeyAsync<PurchaseOrders>(SelectedOrder.Id, "Id");
                SelectedOrder.Project.Client = ProjectClients.Single(c => SelectedOrder.Project.ClientId == c.Id);
                SelectedOrder.ArticlesToPurchase = await ArticlesServerReload(SelectedOrder.Id);
            }
        }
        #endregion
        #endregion

        #region PurchaseArticles
        #region ArticlesTable methods
        private async Task<ICollection<ArticlesToPurchase>> ArticlesServerReload(object OrderId)
        {
            List<ArticlesToPurchase> articles = new();
            DataManagerRequest request = new()
            {
                Where = new()
                {
                    new WhereFilter { Field = "PurchaseOrderId", Value = OrderId.ToString() ?? string.Empty }
                }
            };
            var response = await _articlesService.GetForGridAsync<ArticlesToPurchase>(request, "QuoteId", "DESC");
            if (null != response) articles = response.ToList();
            subtotal = 0f;
            foreach (var item in articles)
            {
                subtotal += item.SalePrice * item.Quantity;
                item.Quote.Article.Family = Families.Single(f => item.Quote.Article.FamilyId == f.Id);
                item.Quote.Article.Family.Group = Groups.Single(g => item.Quote.Article.Family.GroupId == g.Id);
                item.Quote.Article.Code = $"{item.Quote.Article.Family.Group.Code}-{item.Quote.Article.Family.Code}-{item.Quote.Article.Code}";
            }
            discount = subtotal * (SelectedOrder.Discount / 100);
            tax = SelectedOrder.Provider.HasTaxes ? (subtotal - discount) * 0.16f : 0f;
            total = subtotal + tax - discount;
            return articles;
        }

        private void OnSelectedArticle(TableRowClickEventArgs<ArticlesToPurchase> args)
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
                ["Reference"] = SelectedOrder
            };
            var dialog = Dialogs.Show<AddArticlesDialog>("Añadir artículos a orden", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var resultElements = JsonSerializer.Deserialize<HashSet<ArticlesToPurchase>>(result.Data.ToString());
                Snackbar.Add($"{resultElements.Count} artículos han sido añadidos a orden de compra", Severity.Info);                
            }
        }

        private async Task EditArticle(ArticlesToPurchase article)
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

        private async Task DeleteArticle(ArticlesToPurchase article)
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

        private void VewReport()
        {
            _navigationManager.NavigateTo($"/Report/{SelectedOrder.Id}");
        }

        private void RemoveTab(MudTabPanel tabPanel = null, ArticlesToPurchase article = null)
        {
            int? id = (null != tabPanel) ? (int)tabPanel.Tag : null;
            var selectedArticle = article ?? SelectedArticles.FirstOrDefault(p => id == p.QuoteId);
            if (null != selectedArticle) SelectedArticles.Remove(selectedArticle);
        }
    }
}
