using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace MCPackServer.Pages.ProductsModule
{
    public partial class ProductsPage
    {
        #region Dependency Injection
        [Inject]
        public IAuthorizationService _authorizationService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        #endregion

        #region Permission State
        private bool CanCreate, CanEdit, CanDelete = true;
        #region Visible Flags
        private bool VisibleProductInformation = false;
        private bool VisibleCreateProduct, VisibleEditProduct = false;
        #endregion
        #endregion

        #region MudBlazor Components
        MudTable<MCProducts> productsTable = new();
        #endregion

        #region API Elements
        DialogParameters parameters = new();
        #region Search strings
        string TypeFilter = string.Empty;
        string DescriptionFilter = string.Empty;
        string CurrencyFilter = string.Empty;
        string CodeFilter = string.Empty;
        #endregion
        #endregion

        #region Entities and Models
        MCProducts SelectedProduct = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        #region MC-products related methods
        #region Table methods
        private async Task<TableData<MCProducts>> ProductsServerReload(TableState state)
        {
            VisibleProductInformation = false;
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "Type", Value = TypeFilter },
                new WhereFilter { Field = "Code", Value = CodeFilter },
                new WhereFilter { Field = "Description", Value = DescriptionFilter },
                new WhereFilter { Field = "Currency", Value = CurrencyFilter }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<MCProducts>(request, field, order);
            int? count = await _service.GetTotalCountAsync<MCProducts>(request);
            return new TableData<MCProducts>
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedProductRow(TableRowClickEventArgs<MCProducts> args)
        {
            SelectedProduct = args.Item;
            VisibleProductInformation = true;
        }
        #endregion
        private async Task CreateProduct()
        {
            parameters = new()
            {
                ["State"] = ProductsDialog.States.Add,
                ["Model"] = new MCProducts()
            };
            var dialog = Dialogs.Show<ProductsDialog>("Añadir producto", parameters: parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<MCProducts> response = (ActionResponse<MCProducts>)result.Data;
                if (response.IsSuccessful)
                    Snackbar.Add("Producto añadido con éxito.", Severity.Success);
                else
                    Snackbar.Add("Error al añadir producto.", Severity.Error);
                await productsTable.ReloadServerData();
            }
        }
        private async Task EditProduct()
        {
            parameters = new()
            {
                ["State"] = ProductsDialog.States.Edit,
                ["Model"] = SelectedProduct
            };
            var dialog = Dialogs.Show<ProductsDialog>("Editar producto", parameters: parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<MCProducts> response = (ActionResponse<MCProducts>)result.Data;
                if (response.IsSuccessful)
                    Snackbar.Add("Producto editado con éxito.", Severity.Success);
                else
                    Snackbar.Add("Error al editar el producto seleccionado.", Severity.Error);
                await productsTable.ReloadServerData();
            }
        }
        private async Task DeleteProduct()
        {
            parameters = new()
            {
                ["State"] = ProductsDialog.States.Delete,
                ["Model"] = SelectedProduct
            };
            var dialog = Dialogs.Show<ProductsDialog>("Editar producto", parameters: parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<MCProducts> response = (ActionResponse<MCProducts>)result.Data;
                if (response.IsSuccessful)
                {
                    VisibleProductInformation = false;
                    SelectedProduct = new();
                    Snackbar.Add("Producto eliminado con éxito.", Severity.Info);
                }
                else
                    Snackbar.Add("Error al eliminar el producto seleccionado.", Severity.Error);
                await productsTable.ReloadServerData();
            }
        }
        private void DeleteSearchFilters() => TypeFilter = DescriptionFilter = CurrencyFilter = CodeFilter = string.Empty;
        private async Task FilterProducts() => await productsTable.ReloadServerData();
        #endregion
    }
}
