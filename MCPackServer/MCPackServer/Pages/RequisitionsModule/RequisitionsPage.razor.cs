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
        DialogParameters Parameters;
        #endregion
        #region MudTables
        MudTable<Requisitions> RequisitionsTable;
        MudTable<RequisitionArticles> ArticlesTable;
        #endregion
        #region Tabs and properties
        MudTabs RequisitionsInformationTabs;
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
        Requisitions SelectedRequisition = new();
        List<Requisitions> RequisitionsTableItems = new();
        List<RequisitionArticles> SelectedArticles = new();
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
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
        private async Task<TableData<Requisitions>> RequisitionsServerReload(TableState state)
        {
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = new()
                {
                    new WhereFilter { Field = nameof(Requisitions.RequisitionNumber), Value = NumberFilter },
                    new WhereFilter { Field = nameof(Requisitions.UserId), Value = UserIdFilter }
                }
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _requisitionsService.GetForGridAsync<Requisitions>(request, field, order);
            if (null != items) RequisitionsTableItems = items.ToList();
            int? count = await _requisitionsService.GetTotalCountAsync<Requisitions>(request);
            return new TableData<Requisitions>
            {
                Items = RequisitionsTableItems,
                TotalItems = count ?? 0
            };
        }
        private async Task OnSelectedRequisition(TableRowClickEventArgs<Requisitions> args)
        {
            var selectedId = args.Item.Id;
            SelectedRequisition = args.Item;
            SelectedRequisition.RequisitionArticles = await ArticlesServerReload(selectedId);
            VisibleRequisitionInformation = true;
        }
        #endregion
        #region Projects CRUD methods
        private async Task CreateRequisition()
        {
            string number = string.Empty;
            if (null != RequisitionsTableItems && RequisitionsTableItems.Any())
                number = (RequisitionsTableItems.Max(r => int.Parse(r.RequisitionNumber)) + 1).ToString("d5");
            else number = "00001";
            Parameters = new()
            {
                ["State"] = RequisitionsDialog.States.Add,
                ["Model"] = new Requisitions()
                {
                    RequisitionNumber = number,
                    UserId = CurrentUser.Id,
                    IssuedDate = DateTime.Now,
                    RequiredDate = DateTime.Now
                }
            };
            var dialog = Dialogs.Show<RequisitionsDialog>("Añadir nueva requisición", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Requisitions> response = (ActionResponse<Requisitions>)result.Data;
                if (response.IsSuccessful)
                    Snackbar.Add("Requisición añadida con éxito.", Severity.Success);
                else
                    Snackbar.Add("Error al añadir requisición.", Severity.Error);
                await RequisitionsTable.ReloadServerData();
            }
        }
        private async Task EditRequisition(Requisitions model)
        {
            Parameters = new()
            {
                ["State"] = RequisitionsDialog.States.Edit,
                ["Model"] = model
            };
            var dialog = Dialogs.Show<RequisitionsDialog>("Editar requisición", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Requisitions> response = (ActionResponse<Requisitions>)result.Data;
                if (response.IsSuccessful)
                    Snackbar.Add("Requisición editada con éxito.", Severity.Success);
                else
                    Snackbar.Add("Error al editar requisición.", Severity.Error);
                await RequisitionsTable.ReloadServerData();
            }
        }
        private async Task DeleteRequisition(Requisitions model)
        {
            Parameters = new()
            {
                ["State"] = RequisitionsDialog.States.Delete,
                ["Model"] = model
            };
            var dialog = Dialogs.Show<RequisitionsDialog>("Eliminar requisición", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Requisitions> response = (ActionResponse<Requisitions>)result.Data;
                if (response.IsSuccessful)
                {
                    VisibleRequisitionInformation = false;
                    SelectedRequisition = new();
                    Snackbar.Add("Requisición eliminada con éxito.", Severity.Info);
                }
                else
                    Snackbar.Add("Error al eliminar requisición.", Severity.Error);
                await RequisitionsTable.ReloadServerData();
            }
        }
        #endregion
        #endregion

        #region Products related methods
        #region Products table related methdos
        private async Task<ICollection<RequisitionArticles>> ArticlesServerReload(object RequisitionId)
        {
            List<RequisitionArticles> Articles = new();
            DataManagerRequest request = new()
            {
                Where = new List<WhereFilter>()
                {
                    new WhereFilter { Field = "RequisitionId", Value = RequisitionId.ToString() ?? string.Empty }
                }
            };
            var items = await _articlesService.GetForGridAsync<RequisitionArticles>(request, "ArticleId");
            if (null != items) Articles = items.ToList();
            return Articles;
        }

        private void OnSelectedProduct(TableRowClickEventArgs<RequisitionArticles> args)
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
                List<ActionResponse<RequisitionArticles>> resultElements = (List<ActionResponse<RequisitionArticles>>)result.Data;
                SelectedRequisition.RequisitionArticles = await ArticlesServerReload(SelectedRequisition.Id);
                Snackbar.Add($"{resultElements.Count} artículos han sido añadidos a requisición", Severity.Info);
            }
        }
        private async Task EditProduct(RequisitionArticles product)
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
                ActionResponse<RequisitionArticles> response = (ActionResponse<RequisitionArticles>)result.Data;
                if (response.IsSuccessful)
                {
                    SelectedRequisition.RequisitionArticles = await ArticlesServerReload(SelectedRequisition.Id);
                    Snackbar.Add("Artículo editado con éxito.", Severity.Success);
                }
                else
                    Snackbar.Add("Error al editar artículo.", Severity.Error);
            }
        }
        private async Task DeleteProduct(RequisitionArticles product)
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
                ActionResponse<RequisitionArticles> response = (ActionResponse<RequisitionArticles>)result.Data;
                if (response.IsSuccessful)
                {
                    RemoveTab(selectedArticle: product);
                    SelectedRequisition.RequisitionArticles = await ArticlesServerReload(SelectedRequisition.Id);
                    Snackbar.Add("Artículo eliminado con éxito", Severity.Info);
                }
                else
                    Snackbar.Add("Error al eliminar el artículo", Severity.Error);
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
        private void RemoveTab(MudTabPanel? tabPanel = null, RequisitionArticles? selectedArticle = null)
        {
            int? Id = (null != tabPanel) ? (int)tabPanel.Tag : null;
            var product = selectedArticle ?? SelectedArticles.FirstOrDefault(p => Id == p.ArticleId);
            if (null != product) SelectedArticles.Remove(product);
        }
    }
}
