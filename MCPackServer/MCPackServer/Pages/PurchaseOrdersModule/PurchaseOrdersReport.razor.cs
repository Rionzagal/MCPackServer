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
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public IJSRuntime _runtime { get; set; }
        [Parameter]
        public string Id { get; set; }

        private PurchaseOrders Order = new();
        private Providers OrderProvider = new();
        private Projects OrderProject = new();
        private Clients ProjectClient = new();
        private List<ArticlesToPurchase> OrderArticles = new();
        private float Subtotal, Tax, Discount, Total = 0f;
        private List<string> MaterialNumbers = new();
        protected override async Task OnInitializedAsync()
        {
            Order = await _ordersService.GetByKeyAsync<PurchaseOrders>(Id, "Id");
            OrderProvider = Order.Provider;
            OrderProject = Order.Project;
            ProjectClient = await _service.GetByKeyAsync<Clients>(OrderProject.ClientId, "Id");

            #region Get the list of materials of the purchase order
            DataManagerRequest request = new()
            {
                Where = new List<WhereFilter>
                {
                    new WhereFilter { Field = nameof(ArticlesToPurchase.PurchaseOrderId), Value = Id }
                }
            };
            var response = await _articlesService.GetForGridAsync<ArticlesToPurchase>(request, "Quantity");
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
            }
            #endregion
        }

        private void Return()
        {
            _navigationManager.NavigateTo("/PurchaseOrdersManagement");
        }

        private async Task Print()
        {
            await _runtime.InvokeVoidAsync("Print");
        }
    }
}