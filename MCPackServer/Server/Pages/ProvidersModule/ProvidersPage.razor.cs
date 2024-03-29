﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Pages.SharedDialogs;

namespace MCPackServer.Pages.ProvidersModule
{
    public partial class ProvidersPage
    {
        #region Permission State
        #region Permissions
        public bool CanCreateProvider, CanEditProvider, CanDeleteProvider = false;
        public bool CanCreateContact, CanEditContact, CanDeleteContact, CanViewContact = false;
        #endregion
        #region Visible Flags
        public bool VisibleProviderInformation = false;
        #endregion
        #endregion

        #region MudBlazor Components
        #region Tables
        private MudTable<Providers> ProvidersTable = new();
        private MudTable<Contacts> ContactsTable = new();
        #endregion
        #region Dialogs
        DialogParameters Parameters = new();
        #endregion
        #region Panels and Tabs
        MudTabs ProviderInformationTabs = new();
        private int? _selectedContactId;
        #endregion
        #endregion

        #region API Elements
        #region Search strings
        private string MarketNameFilter = string.Empty;
        private string LegalNameFilter = string.Empty;
        private string CityFilter = string.Empty;
        private string ProvinceFilter = string.Empty;
        private string PhoneNumberFilter = string.Empty;
        private string WebsiteFilter = string.Empty;
        #endregion
        #endregion

        #region Entities and Models
        #region Providers
        private Providers NewProvider = new();
        private Providers SelectedProvider = new();
        private Providers EditProviderModel = new();
        #endregion
        #region Contacts
        private Contacts NewContact = new();
        private Contacts EditContactModel = new();
        private List<Contacts> SelectedContacts = new();
        #endregion
        #endregion

        protected override async Task OnInitializedAsync()
        {
            var _authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = _authenticationState.User;
            if (null != user)
            {
                try
                {
                    CanCreateProvider = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Providers.Create)).Succeeded;
                    CanEditProvider = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Providers.Edit)).Succeeded;
                    CanDeleteProvider = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Providers.Delete)).Succeeded;

                    CanViewContact = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Contacts.View)).Succeeded;
                    CanCreateContact = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Contacts.Create)).Succeeded;
                    CanEditContact = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Contacts.Edit)).Succeeded;
                    CanDeleteContact = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Contacts.Delete)).Succeeded;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (_selectedContactId.HasValue)
            {
                ProviderInformationTabs.ActivatePanel(_selectedContactId);
                _selectedContactId = null;
                StateHasChanged();
            }
        }
        #region Providers related methods
        #region Providers table methods
        private async Task<TableData<Providers>> ProvidersServerReload(TableState state)
        {
            VisibleProviderInformation = false;
            List<WhereFilter> filters = new()
            {
                new WhereFilter
                {
                    Field = "MarketName",
                    Value = MarketNameFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                },
                new WhereFilter
                {
                    Field = "LegalName",
                    Value = LegalNameFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                },
                new WhereFilter
                {
                    Field = "City",
                    Value = CityFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                },
                new WhereFilter
                {
                    Field = "Province",
                    Value = ProvinceFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                },
                new WhereFilter
                {
                    Field = "PhoneNumber",
                    Value = PhoneNumberFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                },
                new WhereFilter
                {
                    Field = "Website",
                    Value = WebsiteFilter,
                    Operator = Operators.Contains,
                    Condition = Conditions.And
                }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters,
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<Providers>(request, field, order);
            int? count = await _service.GetTotalCountAsync<Providers>(request);
            return new TableData<Providers>()
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedProviderRow(TableRowClickEventArgs<Providers> args)
        {
            SelectedProvider = args.Item;
            VisibleProviderInformation = true;
        }
        private async Task FilterProviders() => await ProvidersTable.ReloadServerData();
        private void DeleteProviderFilters() =>
            MarketNameFilter = LegalNameFilter = CityFilter
            = ProvinceFilter = PhoneNumberFilter = WebsiteFilter
            = string.Empty;
        #endregion
        #region Providers server methods
        private async Task AddProvider()
        {
            Parameters = new()
            {
                ["State"] = ProvidersDialog.States.Add,
                ["Model"] = new Providers()
            };
            var dialog = Dialogs.Show<ProvidersDialog>("Añadir nuevo proveedor", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ProvidersTable.ReloadServerData();
            }
        }
        private async Task EditProvider()
        {
            Parameters = new()
            {
                ["State"] = ProvidersDialog.States.Edit,
                ["Model"] = SelectedProvider
            };
            var dialog = Dialogs.Show<ProvidersDialog>("Editar proveedor seleccionado", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ProvidersTable.ReloadServerData();
                SelectedProvider = await _service.GetByKeyAsync<Providers>(SelectedProvider.Id);
            }
        }
        private async Task DeleteProvider()
        {
            Parameters = new()
            {
                ["State"] = ProvidersDialog.States.Delete,
                ["Model"] = SelectedProvider
            };
            var dialog = Dialogs.Show<ProvidersDialog>("Eliminar proveedor seleccionado", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ProvidersTable.ReloadServerData();
                VisibleProviderInformation = false;
                SelectedProvider = new();
            }
        }
        #endregion
        #endregion

        #region Contacts related methods
        #region Contacts table methods
        private async Task<TableData<Contacts>> ContactsServerReload(TableState state)
        {
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _providersService.GetContacts(SelectedProvider.Id, request, field, order);
            int? count = await _providersService.CountContacts(SelectedProvider.Id, request);
            return new TableData<Contacts>()
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedContactRow(TableRowClickEventArgs<Contacts> args)
        {
            var selectedId = args.Item.Id;
            if (!SelectedContacts.Any(c => selectedId == c.Id))
                SelectedContacts.Add(args.Item);
            _selectedContactId = selectedId;
        }
        #endregion
        #region Contacts server methods
        private async Task CreateContact()
        {
            Parameters = new()
            {
                ["State"] = ProvidersDialog.States.Add,
                ["Model"] = new Contacts()
            };
            var dialog = Dialogs.Show<ContactsDialog>("Añadir nuevo contacto", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                if (int.TryParse(result.Data.ToString(), out int response))
                {
                    if (0 != response)
                    {
                        var linkResponse = await _providersService.LinkContact(SelectedProvider.Id, response);
                        if (linkResponse.IsSuccessful)
                            Snackbar.Add("Contacto enlazado con éxito.", Severity.Success);
                        else
                            foreach (var error in linkResponse.Errors)
                            {
                                Snackbar.Add(error, Severity.Error);
                            }
                    }
                }
                else
                    Snackbar.Add("Error al revisar respuesta de contacto.", Severity.Error);
                await ContactsTable.ReloadServerData();
            }
        }
        private async Task EditContact(Contacts contact)
        {
            Parameters = new()
            {
                ["State"] = ProvidersDialog.States.Edit,
                ["Model"] = contact
            };
            var dialog = Dialogs.Show<ContactsDialog>("Editar contacto seleccionado", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ContactsTable.ReloadServerData();
            }
        }
        private async Task DeleteContact(Contacts contact)
        {
            Parameters = new()
            {
                ["State"] = ProvidersDialog.States.Delete,
                ["Model"] = contact
            };
            var dialog = Dialogs.Show<ContactsDialog>("Eliminar contacto seleccionado", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                RemoveTab(selectedContact: contact);
                await ContactsTable.ReloadServerData();
            }
        }
        #endregion
        private void RemoveTab(MudTabPanel? tabPanel = null, Contacts? selectedContact = null)
        {
            int? Id = (null != tabPanel) ? (int)tabPanel.Tag : null;
            var contact = selectedContact ?? SelectedContacts.FirstOrDefault(q => (int)tabPanel.Tag == q.Id);
            if (null != contact) SelectedContacts.Remove(contact);
        }
        #endregion
    }
}
