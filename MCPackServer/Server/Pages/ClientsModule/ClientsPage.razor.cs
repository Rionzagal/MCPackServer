﻿using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Pages.SharedDialogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MCPackServer.Pages.ClientsModule
{
    public partial class ClientsPage
    {
        #region Dependency Injection
        #endregion

        #region Permission State
        #region Permissions
        public bool CanCreateClient, CanEditClient, CanDeleteClient = false;
        public bool CanViewContact, CanCreateContact, CanEditContact, CanDeleteContact = false;
        #endregion
        #region Visible Flags
        public bool VisibleClientCreation = false;
        public bool VisibleClientInformation = false;
        public bool VisibleClientEdit = false;
        public bool VisibleContactCreation = false;
        public bool VisibleContactEdit = false;
        public bool VisibleContactSelection = false;
        public bool EnableContactLink = false;
        #endregion
        #endregion

        #region MudBlazor Components
        #region Tables
        private MudTable<Clients> ClientsTable = new();
        private MudTable<Contacts> ContactsTable = new();
        #endregion
        #region Dialogs
        DialogParameters? Parameters;
        #endregion
        #region Tabs and properties
        private MudTabs? ClientInformationTabs;
        private int? _selectedContactId = null;
        #endregion
        #region Forms
        private MudForm? ClientCreationForm, ClientEditForm;
        private MudForm? ContactCreationForm, ContactEditForm;
        #endregion
        #endregion

        #region API Elements
        #region Search strings
        private string? MarketNameFilter = null;
        private string? LegalNameFilter = null;
        private string? CityFilter = null;
        private string? ProvinceFilter = null;
        private string? PhoneNumberFilter = null;
        private string? WebsiteFilter = null;
        #endregion
        #endregion

        #region Entities and Models
        #region Clients
        private Clients NewClient = new();
        private Clients EditClientModel = new();
        private Clients SelectedClient = new();
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
                    CanCreateClient = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Clients.Create)).Succeeded;
                    CanEditClient = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Clients.Edit)).Succeeded;
                    CanDeleteClient = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Clients.Delete)).Succeeded;

                    CanViewContact = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Contacts.View)).Succeeded;
                    CanCreateContact = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Contacts.Create)).Succeeded;
                    CanEditContact = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Clients.Edit)).Succeeded;
                    CanDeleteContact = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Clients.Delete)).Succeeded;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (_selectedContactId.HasValue && ClientInformationTabs != null)
            {
                ClientInformationTabs.ActivatePanel(_selectedContactId);
                _selectedContactId = null;
                StateHasChanged();
            }
        }

        #region Clients related methods
        #region Clients table methods
        private async Task<TableData<Clients>> ClientsServerReload(TableState state)
        {
            VisibleClientInformation = false;
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = new List<WhereFilter>()
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
                }
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            return new TableData<Clients>()
            {
                Items = await _service.GetForGridAsync<Clients>(request, field, order),
                TotalItems = await _service.GetTotalCountAsync<Clients>(request) ?? 0
            };
        }
        private void OnSelectedClientRow(TableRowClickEventArgs<Clients> args)
        {
            SelectedClient = args.Item;
            VisibleClientInformation = true;
        }
        private async Task FilterClients() => await ClientsTable.ReloadServerData();
        private void DeleteClientFilters() =>
            MarketNameFilter = LegalNameFilter = CityFilter = ProvinceFilter
            = PhoneNumberFilter = WebsiteFilter = null;
        #endregion
        #region Clients server methods
        private async Task AddClient()
        {
            Parameters = new()
            {
                ["State"] = ClientsDialog.States.Add,
                ["Model"] = new Clients()
            };
            var dialog = Dialogs.Show<ClientsDialog>("Añadir nuevo cliente", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Clients> Response = (ActionResponse<Clients>)result.Data;
                if (Response.IsSuccessful)
                    Snackbar.Add("Cliente añadido con éxito.", Severity.Success);
                else
                {
                    foreach (var error in Response.Errors)
                    {
                        Snackbar.Add(error, Severity.Error);
                    }
                }
                await ClientsTable.ReloadServerData();
            }
        }
        private async Task EditClient()
        {
            Parameters = new()
            {
                ["State"] = ClientsDialog.States.Edit,
                ["Model"] = SelectedClient
            };
            var dialog = Dialogs.Show<ClientsDialog>("Editar cliente", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Clients> Response = (ActionResponse<Clients>)result.Data;
                if (Response.IsSuccessful)
                    Snackbar.Add("Cliente editado con éxito", Severity.Success);
                else
                {
                    foreach (var error in Response.Errors)
                    {
                        Snackbar.Add(error, Severity.Error);
                    }
                }
                await ClientsTable.ReloadServerData();
            }
        }
        private async Task DeleteClient()
        {
            Parameters = new()
            {
                ["State"] = ClientsDialog.States.Delete,
                ["Model"] = SelectedClient
            };
            var dialog = Dialogs.Show<ClientsDialog>("Eliminar cliente", Parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                ActionResponse<Clients> Response = (ActionResponse<Clients>)result.Data;
                if (Response.IsSuccessful)
                {
                    VisibleClientInformation = false;
                    SelectedClient = new();
                    Snackbar.Add("Cliente eliminado con éxito", Severity.Success);
                }
                else
                {
                    foreach (var error in Response.Errors)
                    {
                        Snackbar.Add(error, Severity.Error);
                    }
                }
                await ClientsTable.ReloadServerData();
            }
        }
        #endregion
        #endregion

        #region Contacts related methods
        #region Client contacts table methods
        private async Task<TableData<Contacts>> ContactsServerReload(TableState state)
        {
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            return new TableData<Contacts>()
            {
                Items = await _clientsService.GetContacts(SelectedClient.Id, request, field, order),
                TotalItems = await _clientsService.CountContacts(SelectedClient.Id, request) ?? 0
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
                ["State"] = ClientsDialog.States.Add,
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
                        var linkResponse = await _clientsService.LinkContact(SelectedClient.Id, response);
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
                ["State"] = ClientsDialog.States.Edit,
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
                ["State"] = ClientsDialog.States.Delete,
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
