using MCPackServer.Entities;
using MCPackServer.Models;
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
        public bool CanCreateClient, CanEditClient, CanDeleteClient = true;
        public bool CanCreateContact, CanEditContact, CanDeleteContact = true;
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
        private string MarketNameFilter = string.Empty;
        private string LegalNameFilter = string.Empty;
        private string CityFilter = string.Empty;
        private string ProvinceFilter = string.Empty;
        private string PhoneNumberFilter = string.Empty;
        private string WebsiteFilter = string.Empty;
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
            await base.OnInitializedAsync();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (_selectedContactId.HasValue)
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
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "MarketName", Value = MarketNameFilter },
                new WhereFilter { Field = "LegalName", Value = LegalNameFilter },
                new WhereFilter { Field = "City", Value = CityFilter },
                new WhereFilter { Field = "Province", Value = ProvinceFilter },
                new WhereFilter { Field = "PhoneNumber", Value = PhoneNumberFilter},
                new WhereFilter { Field = "Website", Value = WebsiteFilter }
            };
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<Clients>(request, field, order);
            int? count = await _service.GetTotalCountAsync<Clients>(request);
            return new TableData<Clients>()
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private async Task OnSelectedClientRow(TableRowClickEventArgs<Clients> args)
        {
            SelectedClient = args.Item;
            VisibleClientInformation = true;
        }
        private async Task FilterClients() => await ClientsTable.ReloadServerData();
        private void DeleteClientFilters()
        {
            MarketNameFilter = LegalNameFilter = CityFilter = ProvinceFilter
            = PhoneNumberFilter = WebsiteFilter = string.Empty;
        }
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
                JObject element = JObject.Parse(result.Data.ToString());
                ActionResponse<Clients> Response = JsonSerializer.Deserialize<ActionResponse<Clients>>(element.ToString());
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
                JObject element = JObject.Parse(result.Data.ToString());
                ActionResponse<Clients> Response = JsonSerializer.Deserialize<ActionResponse<Clients>>(element.ToString());
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
                JObject element = JObject.Parse(result.Data.ToString());
                ActionResponse<Clients> Response = JsonSerializer.Deserialize<ActionResponse<Clients>>(element.ToString());
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
            var items = await _service.GetForGridAsync<Contacts>(request, state.SortLabel);
            int? count = await _service.GetTotalCountAsync<Contacts>(request);
            return new TableData<Contacts>()
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private async Task OnSelectedContactRow(TableRowClickEventArgs<Contacts> args)
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
            //Parameters = new()
            //{
            //    ["State"] = ClientsDialog.States.Add,
            //    ["Model"] = new Contacts()
            //};
            //var dialog = Dialogs.Show<ContactsDialog>("Añadir nuevo contacto", Parameters);
            //var result = await dialog.Result;
            //if (!result.Cancelled)
            //{
            //    var element = JObject.Parse(result.Data.ToString());
            //    if (bool.Parse(element.GetValue("Success").ToString()))
            //    {
            //        Snackbar.Add("Contacto añadido con éxito.", Severity.Success);
            //        var ResultValue = JsonConvert.DeserializeObject<Contacts>
            //            (element.GetValue("Value").ToString());
            //        EntityDBModel<Contacts> LinkPayload = new()
            //        {
            //            Action = "Add",
            //            KeyColumn = nameof(ResultValue.Id),
            //            Key = ResultValue.Id,
            //            Value = ResultValue
            //        };
            //        var response = await Http.PostAsJsonAsync
            //            ($"api/Clients/LinkContact/{SelectedClient.Id}", LinkPayload);
            //        if (response.IsSuccessStatusCode)
            //            Snackbar.Add("Contacto enlazado con éxito.", Severity.Success);
            //        else
            //            Snackbar.Add("Error al enlazar contacto.", Severity.Error);
            //    }
            //    else
            //        Snackbar.Add("Error al añadir contacto.", Severity.Error);
            //    await ContactsTable.ReloadServerData();
            //}
        }
        private async Task EditContact(Contacts contact)
        {
            //Parameters = new()
            //{
            //    ["State"] = ClientsDialog.States.Edit,
            //    ["Model"] = contact
            //};
            //var dialog = Dialogs.Show<ContactsDialog>("Editar contacto seleccionado", Parameters);
            //var result = await dialog.Result;
            //if (!result.Cancelled)
            //{
            //    var element = JObject.Parse(result.Data.ToString());
            //    if (bool.Parse(element.GetValue("Success").ToString()))
            //        Snackbar.Add("Contacto editado con éxito.", Severity.Success);
            //    else
            //        Snackbar.Add("Error al editar el contacto seleccionado.", Severity.Error);
            //    await ContactsTable.ReloadServerData();
            //}
        }
        private async Task DeleteContact(Contacts contact)
        {
            //Parameters = new()
            //{
            //    ["State"] = ClientsDialog.States.Delete,
            //    ["Model"] = contact
            //};
            //var dialog = Dialogs.Show<ContactsDialog>("Eliminar contacto seleccionado", Parameters);
            //var result = await dialog.Result;
            //if (!result.Cancelled)
            //{
            //    var element = JObject.Parse(result.Data.ToString());
            //    if (bool.Parse(element.GetValue("Success").ToString()))
            //    {
            //        Snackbar.Add("Contacto eliminado con éxito.", Severity.Info);
            //        RemoveTab(selectedContact: contact);
            //    }
            //    else
            //        Snackbar.Add("Error al eliminar contacto seleccionado.", Severity.Error);
            //    await ContactsTable.ReloadServerData();
            //}
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
