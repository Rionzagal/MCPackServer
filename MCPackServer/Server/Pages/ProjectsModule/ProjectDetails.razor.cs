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

        // Mud Components
        private MudTable<ProjectProductsView> ProductsTable = new();
        private MudTable<PurchaseOrdersView> POTable = new();
        private List<ArticlesToPurchaseView> ProjectATPs = new();
        private HashSet<int> _selectedPurchaseOrderIds = new();

        // Temporal variables


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
            }
        }
        #endregion
        
        #region Purchase orders
        private async Task<TableData<PurchaseOrdersView>> POServerReload(TableState state)
        {
            // * Reload the Purchase Orders associated to the current project.
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

            // * Reload the Articles of each Purchase Order in the list of Purchase Orders.
            foreach (var id in items.Select(x => x.Id))
            {
                try
                {
                    DataManagerRequest ATPrequest = new()
                    {
                        Where = new List<WhereFilter>()
                        {
                            new WhereFilter { Field=nameof(ArticlesToPurchaseView.PurchaseOrderId), Value=id, Operator=Operators.Equal }
                        }
                    };
                    // ATPrequest.Take = (await _service.GetTotalCountAsync<ArticlesToPurchaseView>(ATPrequest)) ?? 0;
                    ProjectATPs.AddRange(await _service.GetForGridAsync<ArticlesToPurchaseView>(ATPrequest, sortField: nameof(ArticlesToPurchaseView.Quantity), getAll: true));
                }
                catch (System.Exception ex)
                {
                    Snackbar.Add("Algo salió mal al recuperar los artículos de las órdenes de compra.", Severity.Error);
                    Console.WriteLine(ex.ToString());
                }
            }

            return new TableData<PurchaseOrdersView>
            {
                Items = items ?? new List<PurchaseOrdersView>(),
                TotalItems = count ?? 0
            };
        }

        private void OnSelectedPO(TableRowClickEventArgs<PurchaseOrdersView> args) => _selectedPurchaseOrderIds.Add(args.Item.Id);

        #endregion
    }
}