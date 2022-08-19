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
        private List<ArticlesView> ArticlesInfo = new();
        private double Subtotal, Tax, Discount, Total = 0f;
        private List<string> MaterialNumbers = new();
        private int NumberOfPages = 1;
        private List<int> ArticlesPerPage = new();
        private string UserFullName = string.Empty;

        private bool _loading = true;
        protected override async Task OnInitializedAsync()
        {
            _loading = true;
            if (string.IsNullOrEmpty(Id))
                Return();
            else
            {
                Order = await _service.GetByKeyAsync<PurchaseOrders>(Id, nameof(PurchaseOrders.Id));
                OrderProvider = await _service.GetByKeyAsync<Providers>(Order.ProviderId, nameof(Providers.Id));
                OrderProject = await _service.GetByKeyAsync<Projects>(Order.ProjectId, nameof(Projects.Id));
                ProjectClient = await _service.GetByKeyAsync<Clients>(OrderProject.ClientId, nameof(Clients.Id));

                try
                {
                    string userName = "compras@mc-pack.com";
                    if (!string.IsNullOrEmpty(userName))
                    {
                        var userInfo = await _service.GetByKeyAsync<UserPersonalInformationView>
                            (userName, nameof(UserPersonalInformationView.UserName));
                        UserFullName = userInfo?.FullName ?? "A quien corresponda.";
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    UserFullName = "Compras";
                }

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
                    ArticlesPerPage = new();
                    for (int i = 0; i < NumberOfPages; i++)
                    {
                        int ArticlesInPage = OrderArticles.Count < 10 * (i + 1) ? OrderArticles.Count % 10 : 10;
                        ArticlesPerPage.Add(ArticlesInPage);
                    }
                    foreach (var item in OrderArticles)
                        ArticlesInfo.Add(await _service.GetByKeyAsync<ArticlesView>(item.ArticleCode, nameof(ArticlesView.Code)));
                }
                #endregion
            }
            _loading = false;
        }

        private void Return()
        {
            _navigationManager?.NavigateTo("PurchaseOrders");
        }

        private async Task Print()
        {
            _loading = true;
            if (null != _runtime)
            {
                var day = DateTime.Now.Day;
                var month = DateTime.Now.Month;
                var hour = DateTime.Now.Hour;
                var minute = DateTime.Now.Minute;
                string genDate = $"{day}{month}_{hour}h{minute}";
                string filename = $"OC{Order.OrderNumber}_{genDate}.pdf";
                await _runtime.InvokeVoidAsync("makePDF", "page", filename);
            }
            else
                return;
            _loading = false;
        }
    }
}
