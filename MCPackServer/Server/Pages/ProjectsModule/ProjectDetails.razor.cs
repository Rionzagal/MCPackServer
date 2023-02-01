using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCPackServer.Pages.ProjectsModule
{
    public partial class ProjectDetails
    {
        [Inject]
        public NavigationManager? _navigationManager { get; set; }
        [Inject]
        public IJSRuntime? JSRuntime { get; set; }
        [Parameter]
        public string? Number { get; set; }

        // Attributes and variables
        private ProjectsView CurrentProject = new();
        private Clients ProjectClient = new();
        private List<ProjectProductsView> SelectedProducts = new();
        private List<PurchaseOrdersView> ProjectPOs = new();

        // Mud Components
        private MudTable<ProjectProductsView> ProductsTable = new();
        private MudTable<PurchaseOrdersView> POTable = new();

        // Temporal variables
        private int? _selectedProductId;

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(Number) || !int.TryParse(Number, out int result))
            {
                Snackbar.Add("Número de proyecto no encontrado o no válido. Regresando a página principal.", Severity.Warning);
                _navigationManager?.NavigateTo("Projects");
            }
            else
            {
                CurrentProject = await _service.GetByKeyAsync<ProjectsView>(Number, key: nameof(ProjectsView.ProjectNumber));
                if (0 == CurrentProject.Id)
                {
                    Snackbar.Add("Proyecto no encontrado.", Severity.Error);
                    _navigationManager?.NavigateTo("Projects");
                }
            }
            ProjectClient = await _service.GetByKeyAsync<Clients>(CurrentProject.ClientId);
        }

        #region Project products
        private async Task<TableData<ProjectProductsView>> ProductsServerReload(TableState state)
        {
            List<WhereFilter> filters = new()
            {
                new WhereFilter
                {
                    Field = nameof(ProjectProducts.ProjectId),
                    Value = CurrentProject.Id,
                    Operator = Operators.Equal
                }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters
            };
            string field = state.SortLabel ?? nameof(ProjectProductsView.ProductId);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            return new TableData<ProjectProductsView>()
            {
                Items = await _service.GetForGridAsync<ProjectProductsView>(request, field, order),
                TotalItems = await _service.GetTotalCountAsync<ProjectProductsView>(request, nameof(ProjectProductsView.ProductId))
                    ?? 0
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
        
        #region Purchase orders
        private async Task<TableData<PurchaseOrdersView>> POServerReload(TableState state)
        {
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = new List<WhereFilter>()
                {
                    new WhereFilter { Field=nameof(PurchaseOrdersView.ProjectId), Value=CurrentProject.Id, Operator=Operators.Equal}
                }
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
        #endregion
    }
}