using MCPackServer.Entities;
using MCPackServer.Models;
using MudBlazor;

namespace MCPackServer.Pages.ProjectsModule
{
    public partial class ProjectsPage
    {
        #region Permissions and Flags
        #region Permissions
        private bool CanCreateProjects = true;
        private bool CanEditProjects = true;
        private bool CanDeleteProjects = true;
        #endregion
        #region Visible flags
        private bool VisibleProjectInformation = false;
        #endregion
        #endregion

        #region MudBlazor Components
        #region Dialogs
        DialogParameters Parameters = new();
        #endregion
        #region MudTables
        MudTable<ProjectsView> ProjectsTable = new();
        MudTable<ProjectProductsView> ProductsTable = new();
        #endregion
        #region Tabs and properties
        MudTabs ProjectsInformationTabs = new();
        private int? _selectedProductId;
        #endregion
        #endregion

        #region Search filters
        private int? ClientIdFilter = null;
        private string NumberFilter = string.Empty;
        private string TypeFilter = string.Empty;
        private int? ProductIdFilter = null;
        #endregion

        #region Entities and models
        ProjectsView SelectedProject = new();
        double subtotal = 0f, tax = 0f, total = 0f, commission = 0f, discount = 0f;
        List<ProjectProductsView> SelectedProducts = new();
        List<Clients> ClientsList = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await ClientsServerReload();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (_selectedProductId.HasValue)
            {
                ProjectsInformationTabs.ActivatePanel(_selectedProductId);
                _selectedProductId = null;
                StateHasChanged();
            }
        }

        #region Projects related methods
        #region Projects table related methods
        private async Task<TableData<ProjectsView>> ProjectsServerReload(TableState state)
        {
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = nameof(ProjectsView.ClientId), Value = ClientIdFilter?.ToString() ?? string.Empty },
                new WhereFilter { Field = nameof(ProjectsView.ProjectNumber), Value = NumberFilter ?? string.Empty },
                new WhereFilter { Field = nameof(ProjectsView.Type), Value = TypeFilter ?? string.Empty }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters,
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<ProjectsView>(request, field, order);
            int? count = await _service.GetTotalCountAsync<ProjectsView>(request);
            return new TableData<ProjectsView>
            {
                Items = items ?? new List<ProjectsView>(),
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedProject(TableRowClickEventArgs<ProjectsView> args)
        {
            SelectedProject = args.Item;
            VisibleProjectInformation = true;
        }
        #endregion
        #region Projects CRUD methods
        private async Task CreateProject()
        {
            Parameters = new()
            {
                ["State"] = ProjectsDialog.States.Add
            };
            var dialog = Dialogs.Show<ProjectsDialog>("Añadir nuevo proyecto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ProjectsTable.ReloadServerData();
                DataManagerRequest dm = new() { Take = 1 };
                var projects = await _service.GetForGridAsync<ProjectsView>(dm, "Id", order: "DESC");
                SelectedProject = projects?.First() ?? new();
                VisibleProjectInformation = SelectedProject.Id != 0;
            }
        }
        private async Task EditProject()
        {
            Parameters = new()
            {
                ["State"] = ProjectsDialog.States.Edit,
                ["ModelView"] = SelectedProject
            };
            var dialog = Dialogs.Show<ProjectsDialog>("Editar proyecto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ProjectsTable.ReloadServerData();
                SelectedProject = ProjectsTable.Items.Single(p => p.Id == SelectedProject.Id);
            }
        }
        private async Task DeleteProject()
        {
            Parameters = new()
            {
                ["State"] = ProjectsDialog.States.Delete,
                ["ModelView"] = SelectedProject
            };
            var dialog = Dialogs.Show<ProjectsDialog>("Eliminar proyecto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                VisibleProjectInformation = false;
                SelectedProject = new();
                await ProjectsTable.ReloadServerData();
            }
        }
        #endregion
        #endregion

        #region Products related methods
        #region Products table related methdos
        private async Task<TableData<ProjectProductsView>> ProductsServerReload(TableState state)
        {
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = nameof(ProjectProducts.ProjectId), Value = SelectedProject.Id.ToString() }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters
            };
            string field = state.SortLabel ?? nameof(ProjectProductsView.ProductId);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<ProjectProductsView>(request, field, order);
            int? count = await _service.GetTotalCountAsync<ProjectProductsView>(request, nameof(ProjectProductsView.ProductId));
            if (null != items)
            {
                subtotal = 0f;
                foreach (var product in items)
                {
                    subtotal += product.SalePrice * product.Quantity;
                }
                discount = SelectedProject.Discount * subtotal;
                tax = SelectedProject.HasTaxes ? (subtotal - discount) * 0.16f : 0f;
                total = subtotal - discount + tax;
            }
            return new TableData<ProjectProductsView>()
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }

        private void OnSelectedProduct(TableRowClickEventArgs<ProjectProductsView> args)
        {
            var selectedId = args.Item.ProductId;
            if (!SelectedProducts.Any(p => selectedId == p.ProductId))
            {
                SelectedProducts.Add(args.Item);
                _selectedProductId = selectedId;
            }
        }
        #endregion
        #region Products CRUD methods
        private async Task AddProduct()
        {
            Parameters = new()
            {
                ["State"] = ProjectProductsDialog.States.Add,
                ["ProjectId"] = SelectedProject.Id
            };
            var dialog = Dialogs.Show<ProjectProductsDialog>("Añadir producto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ProductsTable.ReloadServerData();
            }
        }
        private async Task EditProduct(ProjectProductsView product)
        {
            Parameters = new()
            {
                ["State"] = ProjectProductsDialog.States.Edit,
                ["ModelView"] = product
            };
            var dialog = Dialogs.Show<ProjectProductsDialog>("Editar producto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                RemoveTab(selectedProduct: product);
                await ProductsTable.ReloadServerData();
            }
        }
        private async Task DeleteProduct(ProjectProductsView product)
        {
            Parameters = new()
            {
                ["State"] = ProjectProductsDialog.States.Delete,
                ["ModelView"] = product
            };
            var dialog = Dialogs.Show<ProjectProductsDialog>("Eliminar producto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                RemoveTab(selectedProduct: product);
                await ProductsTable.ReloadServerData();
            }
        }
        #endregion
        #endregion

        private async Task<IEnumerable<int?>> ClientsServerReload(string filter = "")
        {
            List<int?> result = new();
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "MarketName", Value = filter }
            };
            DataManagerRequest dm = new()
            {
                Where = filters,
            };
            var response = await _service.GetForGridAsync<Clients>(dm);
            if (null != response && response.Any())
            {
                ClientsList = response.ToList();
                foreach (var client in ClientsList)
                {
                    result.Add(client.Id);
                }
            }
            return result;
        }
        private void RemoveTab(MudTabPanel? tabPanel = null, ProjectProductsView? selectedProduct = null)
        {
            int? Id = (null != tabPanel) ? (int)tabPanel.Tag : null;
            var product = selectedProduct ?? SelectedProducts.First(p => Id == p.ProductId);
            if (null != product) SelectedProducts.Remove(product);
        }

        private string GetClientName(int? Id)
        {
            string name = "";
            if (Id.HasValue)
            {
                var match = ClientsList.FirstOrDefault(c => Id == c.Id);
                if (null != match) name = match.MarketName;
            }
            return name;
        }
    }
}
