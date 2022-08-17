﻿using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCPackServer.Pages.PurchaseOrdersModule
{
    public partial class PurchaseOrdersReport
    {
        [Inject]
        public NavigationManager? _navigationManager { get; set; }
        [Inject]
        public IJSRuntime? _runtime { get; set; }
        [Parameter]
        public string? Id { get; set; }

        private PurchaseOrders Order = new();
        private Providers OrderProvider = new();
        private Projects OrderProject = new();
        private Clients ProjectClient = new();
        private List<ArticlesToPurchaseView> OrderArticles = new();
        private double Subtotal, Tax, Discount, Total = 0f;
        private List<string> MaterialNumbers = new();
        private int NumberOfPages = 1;
        private List<int> ArticlesPerPage = new();
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(Id))
                Return();
            else
            {
                Order = await _service.GetByKeyAsync<PurchaseOrders>(Id, nameof(PurchaseOrders.Id));
                OrderProvider = await _service.GetByKeyAsync<Providers>(Order.ProviderId, nameof(Providers.Id));
                OrderProject = await _service.GetByKeyAsync<Projects>(Order.ProjectId, nameof(Projects.Id));
                ProjectClient = await _service.GetByKeyAsync<Clients>(OrderProject.ClientId, nameof(Clients.Id));

                #region Get the list of materials of the purchase order
                DataManagerRequest request = new()
                {
                    Where = new List<WhereFilter>
                    {
                        new WhereFilter { Field = nameof(ArticlesToPurchase.PurchaseOrderId), Value = Id }
                    }
                };
                var response = await _service.GetForGridAsync<ArticlesToPurchaseView>(request, "Quantity", getAll: true);
                if (null != response)
                {
                    OrderArticles = response.ToList();
                    for (int i = 0; i < OrderArticles.Count; i++)
                    {
                        MaterialNumbers.Add((i + 1).ToString());
                        Subtotal += OrderArticles[i].SalePrice * OrderArticles[i].Quantity;
                    }
                    Discount = Subtotal * (Order.Discount / 100);
                    Tax = OrderProvider.HasTaxes ? (Subtotal - Discount) * 0.16f : 0;
                    Total = Subtotal - Discount + Tax;
                    NumberOfPages = 1 + OrderArticles.Count / 10;
                    for (int i = 0; i < NumberOfPages; i++)
                    {
                        var ArticlesInPage = OrderArticles.Count < 10 * (i + 1) ? OrderArticles.Count % 10 : 10;
                        ArticlesPerPage.Append(ArticlesInPage);
                    }
                }
                #endregion
            }
        }

        private void Return()
        {
            _navigationManager?.NavigateTo("PurchaseOrders");
        }

        private async Task Print()
        {
            if (null != _runtime)
                await _runtime.InvokeVoidAsync("saveAsPDF", "DocumentBody", "test.pdf");
            else
                return;
        }
    }
}
