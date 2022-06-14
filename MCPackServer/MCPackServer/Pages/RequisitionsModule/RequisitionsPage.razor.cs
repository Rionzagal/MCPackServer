using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MCPackServer.Pages.RequisitionsModule
{
    public partial class RequisitionsPage
    {
        #region Permissions and Flags
        #region Permissions
        private bool CanCreate;
        private bool CanEdit;
        private bool CanDelete;
        #endregion
        #region Visible flags
        private bool VisibleRequisitionInformation;
        #endregion
        #endregion

        #region MudBlazor Components
        #region Dialogs
        DialogParameters Parameters = new();
        #endregion
        #region MudTables
        MudTable<RequisitionsView> RequisitionsTable = new();
        MudTable<RequisitionArticlesView> ArticlesTable = new();
        #endregion
        #region Tabs and properties
        MudTabs RequisitionsInformationTabs = new();
        private int? _selectedArticleId;
        #endregion
        #endregion

        #region API elements
        #region Datamanager requests
        DataManagerRequest RequisitionsDm;
        DataManagerRequest ArticlesDm;
        #endregion
        #region Search filters
        private string UserIdFilter = string.Empty;
        private string NumberFilter = string.Empty;
        #endregion
        #endregion

        #region Entities and models
        AspNetUsers CurrentUser = new();
        RequisitionsView SelectedRequisition = new();
        List<RequisitionsView> RequisitionsTableItems = new();
        List<RequisitionArticlesView> SelectedArticles = new();
        List<UserInformationView> UsersList = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            var AuthenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = AuthenticationState.User;
            if (null != user)
            {
                try
                {
                    CanCreate = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Requisitions.Create)).Succeeded;
                    CanEdit = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Requisitions.Edit)).Succeeded;
                    CanDelete = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Requisitions.Delete)).Succeeded;

                    CurrentUser = await _service.GetByKeyAsync<AspNetUsers>(user?.Identity?.Name ?? string.Empty, "UserName");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (_selectedArticleId.HasValue)
            {
                RequisitionsInformationTabs.ActivatePanel(_selectedArticleId);
                _selectedArticleId = null;
                StateHasChanged();
            }
        }

        #region Requisitions related methods
        #region Requisitions table related methods
        private async Task<TableData<RequisitionsView>> RequisitionsServerReload(TableState state)
        {
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = new()
                {
                    new WhereFilter { Field = nameof(RequisitionsView.RequisitionNumber), Value = NumberFilter },
                    new WhereFilter { Field = nameof(RequisitionsView.UserId), Value = UserIdFilter }
                }
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<RequisitionsView>(request, field, order);
            if (null != items) 
                RequisitionsTableItems = items.ToList();
            int? count = await _service.GetTotalCountAsync<RequisitionsView>(request);
            return new TableData<RequisitionsView>
            {
                Items = RequisitionsTableItems,
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedRequisition(TableRowClickEventArgs<RequisitionsView> args)
        {
            SelectedRequisition = args.Item;
            VisibleRequisitionInformation = true;
        }
        #endregion
        #region Requisitions CRUD methods
        private async Task CreateRequisition()
        {
            Parameters = new()
            {
                ["State"] = RequisitionsDialog.States.Add,
                ["UserId"] = CurrentUser.Id
            };
            var dialog = Dialogs.Show<RequisitionsDialog>("Añadir nueva requisición", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await RequisitionsTable.ReloadServerData();
                SelectedRequisition = (await _service.GetForGridAsync<RequisitionsView>
                    (new() { Take = 1 }, order: "DESC"))
                    .First();
                VisibleRequisitionInformation = true;
            }
        }
        private async Task EditRequisition(RequisitionsView model)
        {
            Parameters = new()
            {
                ["State"] = RequisitionsDialog.States.Edit,
                ["ModelView"] = model
            };
            var dialog = Dialogs.Show<RequisitionsDialog>("Editar requisición", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await RequisitionsTable.ReloadServerData();
                SelectedRequisition = await _service.GetByKeyAsync<RequisitionsView>(SelectedRequisition.Id);
            }
        }
        private async Task DeleteRequisition(RequisitionsView model)
        {
            Parameters = new()
            {
                ["State"] = RequisitionsDialog.States.Delete,
                ["ModelView"] = model
            };
            var dialog = Dialogs.Show<RequisitionsDialog>("Eliminar requisición", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await RequisitionsTable.ReloadServerData();
                VisibleRequisitionInformation = false;
                SelectedRequisition = new();
            }
        }
        #endregion
        #endregion

        #region Products related methods
        #region Products table related methdos
        private async Task<TableData<RequisitionArticlesView>> ArticlesServerReload(TableState state)
        {
            List<RequisitionArticlesView> Articles = new();
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = new List<WhereFilter>()
                {
                    new WhereFilter { Field = "RequisitionId", Value = SelectedRequisition.Id.ToString() }
                }
            };
            string field = state.SortLabel ?? nameof(RequisitionArticlesView.ArticleId);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<RequisitionArticlesView>(request, field, order);
            int count = await _service.GetTotalCountAsync<RequisitionArticlesView>(request, nameof(RequisitionArticles.ArticleId))
                ?? 0;
            if (null != items) 
                Articles = items.ToList();
            return new TableData<RequisitionArticlesView>
            {
                Items = Articles,
                TotalItems = count
            };
        }

        private void OnSelectedProduct(TableRowClickEventArgs<RequisitionArticlesView> args)
        {
            var selectedId = args.Item.ArticleId;
            if (!SelectedArticles.Any(p => selectedId == p.ArticleId))
            {
                SelectedArticles.Add(args.Item);
                _selectedArticleId = selectedId;
            }
        }
        #endregion
        #region Products CRUD methods
        private async Task AddProduct()
        {
            Parameters = new()
            {
                ["Reference"] = SelectedRequisition
            };
            var dialog = Dialogs.Show<AddArticleDialog>("Añadir artículos a requisición", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
            }
        }
        private async Task EditProduct(RequisitionArticlesView product)
        {
            Parameters = new()
            {
                ["State"] = RequisitionArticlesDialog.States.Edit,
                ["Reference"] = SelectedRequisition,
                ["Model"] = product
            };
            var dialog = Dialogs.Show<RequisitionArticlesDialog>("Editar artículo", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                
            }
        }
        private async Task DeleteProduct(RequisitionArticlesView product)
        {
            Parameters = new()
            {
                ["State"] = RequisitionArticlesDialog.States.Delete,
                ["Reference"] = SelectedRequisition,
                ["Model"] = product
            };
            var dialog = Dialogs.Show<RequisitionArticlesDialog>("Eliminar producto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                
            }
        }
        #endregion
        #endregion

        private async Task<IEnumerable<string>> UsersServerReload(string filter)
        {
            List<string> result = new();
            DataManagerRequest dm = new()
            {
                Where = new()
                {
                    new WhereFilter { Field = nameof(AspNetUsers.UserName), Value = filter }
                }
            };
            var items = await _service.GetForGridAsync<UserInformationView>(dm);
            if (null != items)
            {
                UsersList = items.ToList();
                UsersList.ForEach(c => result.Add(c.Id));
            }
            return result;
        }

        private string GetUserName(string Id)
        {
            string UserName = string.Empty;
            if (!string.IsNullOrEmpty(Id))
            {
                var match = UsersList.FirstOrDefault(c => c.Id == Id);
                if (match != null) UserName = match.UserName;
            }
            return UserName;
        }
        private void RemoveTab(MudTabPanel? tabPanel = null, RequisitionArticlesView? selectedArticle = null)
        {
            int? Id = (null != tabPanel) ? (int)tabPanel.Tag : null;
            var product = selectedArticle ?? SelectedArticles.FirstOrDefault(p => Id == p.ArticleId);
            if (null != product) SelectedArticles.Remove(product);
        }
    }
}
