using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace MCPackServer.Pages.ProjectsModule
{
    public partial class ProjectsPage
    {
        #region Dependency Injection
        #endregion

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
        MudTable<Projects> ProjectsTable = new();
        MudTable<ProjectProducts> ProductsTable = new();
        #endregion
        #region Tabs and properties
        MudTabs ProjectsInformationTabs = new();
        private int? _selectedProductId;
        #endregion
        #endregion

        #region Search filters
        private int? ClientIdFilter = null;
        private string DescriptionFilter = string.Empty;
        private int? ProductIdFilter = null;
        #endregion

        #region Entities and models
        Projects SelectedProject = new();
        float subtotal = 0f, tax = 0f, total = 0f, commission = 0f, discount = 0f;
        List<ProjectProducts> SelectedProducts = new();
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
        private async Task<TableData<Projects>> ProjectsServerReload(TableState state)
        {
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "ClientId", Value = ClientIdFilter.HasValue ? ClientIdFilter.Value.ToString() : string.Empty },
                new WhereFilter { Field = "Description", Value = DescriptionFilter }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters,
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _projectsService.GetForGridAsync<Projects>(request, field, order);
            int? count = await _projectsService.GetTotalCountAsync<Projects>(request);
            return new TableData<Projects>
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedProject(TableRowClickEventArgs<Projects> args)
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
                ["State"] = ProjectsDialog.States.Add,
                ["Model"] = new Projects()
            };
            var dialog = Dialogs.Show<ProjectsDialog>("Añadir nuevo proyecto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Projects> response = (ActionResponse<Projects>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Proyecto añadido con éxito.", Severity.Success);
                }
                else
                    Snackbar.Add("Error al añadir proyecto.", Severity.Error);
                await ProjectsTable.ReloadServerData();
            }
        }
        private async Task EditProject()
        {
            Parameters = new()
            {
                ["State"] = ProjectsDialog.States.Edit,
                ["Model"] = SelectedProject
            };
            var dialog = Dialogs.Show<ProjectsDialog>("Editar proyecto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Projects> response = (ActionResponse<Projects>)result.Data;
                if (response.IsSuccessful)
                {
                    VisibleProjectInformation = false;
                    SelectedProject = new();
                    Snackbar.Add("Proyecto editado con éxito.", Severity.Success);
                }
                else
                    Snackbar.Add("Error al editar proyecto.", Severity.Error);
                await ProjectsTable.ReloadServerData();
            }
        }
        private async Task DeleteProject()
        {
            Parameters = new()
            {
                ["State"] = ProjectsDialog.States.Delete,
                ["Model"] = SelectedProject
            };
            var dialog = Dialogs.Show<ProjectsDialog>("Eliminar proyecto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Projects> response = (ActionResponse<Projects>)result.Data;
                if (response.IsSuccessful)
                {
                    VisibleProjectInformation = false;
                    SelectedProject = new();
                    Snackbar.Add("Proyecto eliminado con éxito.", Severity.Info);
                }
                else
                    Snackbar.Add("Error al eliminar proyecto.", Severity.Error);
                await ProjectsTable.ReloadServerData();
            }
        }
        #endregion
        #endregion

        #region Products related methods
        #region Products table related methdos
        private async Task<ICollection<ProjectProducts>> ProductsServerReload(int? ProjectId)
        {
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "ProjectId", Value = ProjectId.HasValue ? ProjectId.Value.ToString() : string.Empty }
            };
            DataManagerRequest request = new()
            {
                Where = filters
            };
            var items = await _productsService.GetForGridAsync<ProjectProducts>(request, "ProductId");
            int? count = await _productsService.GetTotalCountAsync<ProjectProducts>(request);
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
                return items.ToList();
            }
            return new List<ProjectProducts>();
        }

        private void OnSelectedProduct(TableRowClickEventArgs<ProjectProducts> args)
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
                ["Model"] = new ProjectProducts { ProjectId = SelectedProject.Id }
            };
            var dialog = Dialogs.Show<ProjectProductsDialog>("Añadir producto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<ProjectProducts> response = (ActionResponse<ProjectProducts>)result.Data;
                if (response.IsSuccessful)
                {
                    Snackbar.Add("Producto añadido con éxito.", Severity.Success);
                }
                else
                    Snackbar.Add("Error al añadir producto.", Severity.Error);
                SelectedProject.ProjectProducts = await ProductsServerReload(SelectedProject.Id);
            }
        }
        private async Task EditProduct(ProjectProducts product)
        {
            Parameters = new()
            {
                ["State"] = ProjectProductsDialog.States.Edit,
                ["Model"] = product
            };
            var dialog = Dialogs.Show<ProjectProductsDialog>("Editar producto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<ProjectProducts> response = (ActionResponse<ProjectProducts>)result.Data;
                if (response.IsSuccessful)
                {
                    RemoveTab(selectedProduct: product);
                    SelectedProject.ProjectProducts = await ProductsServerReload(SelectedProject.Id);
                    Snackbar.Add("Producto editado con éxito.", Severity.Success);
                }
                else
                    Snackbar.Add("Error al editar producto.", Severity.Error);
            }
        }
        private async Task DeleteProduct(ProjectProducts product)
        {
            Parameters = new()
            {
                ["State"] = ProjectProductsDialog.States.Delete,
                ["Model"] = product
            };
            var dialog = Dialogs.Show<ProjectProductsDialog>("Eliminar producto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<ProjectProducts> response = (ActionResponse<ProjectProducts>)result.Data;
                if (response.IsSuccessful)
                {
                    RemoveTab(selectedProduct: product);
                    SelectedProject.ProjectProducts = await ProductsServerReload(SelectedProject.Id);
                    Snackbar.Add("Producto eliminado con éxito", Severity.Info);
                }
                else
                    Snackbar.Add("Error al eliminar el producto", Severity.Error);
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
        private void RemoveTab(MudTabPanel? tabPanel = null, ProjectProducts? selectedProduct = null)
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
