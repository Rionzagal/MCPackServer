using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCPackServer.Pages.ClientsModule
{
    public partial class ClientsDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public Clients Model { get; set; } = new();
        #endregion

        #region Dialog variables
        private string Title;
        private string TitleIcon;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private MudForm Form;
        private List<Contacts> ClientContacts = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            //if ((int)States.Add == State) //representing an Add dialog
            //{
            //    Title = "Añadir nuevo cliente";
            //    TitleIcon = Icons.Material.Filled.Create;
            //    Disabled = false;
            //    ButtonColor = Color.Success;
            //    Client = new();
            //    Request = "api/Clients/Add";
            //}
            //else if ((int)States.Edit == State) //representing an Edit dialog
            //{
            //    Title = "Editar cliente seleccionado";
            //    TitleIcon = Icons.Material.Filled.Edit;
            //    Disabled = false;
            //    ButtonColor = Color.Primary;
            //    Request = "api/Clients/Update";
            //}
            //else if ((int)States.Delete == State) //representing a Delete dialog
            //{
            //    _processing = true;
            //    DataManagerRequest contactsRequest = new()
            //    {
            //        Take = 0,
            //        Skip = 0,
            //        RequiresCounts = true
            //    };
            //    Title = "Eliminar cliente seleccionado";
            //    TitleIcon = Icons.Material.Filled.Delete;
            //    Disabled = true;
            //    ButtonColor = Color.Error;
            //    Request = "api/Clients/Remove";
            //    var contactsResponse = await Http.PostAsJsonAsync($"api/Clients/GetContacts/{Client.Id}", contactsRequest);
            //    if (contactsResponse.IsSuccessStatusCode)
            //    {
            //        JObject element = JObject.Parse(await contactsResponse.Content.ReadAsStringAsync());
            //        ClientContacts = JsonSerializer.Deserialize<IEnumerable<Contacts>>
            //            (element.GetValue("Result")
            //            .ToString())
            //            .ToList();
            //    }
            //    else
            //        Snackbar.Add("Error al cargar los contactos asociados al cliente.", Severity.Error);
            //    _processing = false;
            //}
            //else //should not get to this option
            //{
            //    Title = null;
            //    TitleIcon = null;
            //    Disabled = true;
            //    ButtonColor = Color.Default;
            //    Dialog.Cancel();
            //}
        }

        private async Task Submit()
        {
            ActionResponse<Clients> response = new();
            _processing = true;
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State) response = await _clientsService.AddAsync(Model);
                else if (States.Edit == State) response = await _clientsService.UpdateAsync(Model);
                else
                {
                    var clearResponse = await _clientsService.ClearContacts(Model.Id);
                    if (clearResponse.IsSuccessful) response = await _clientsService.RemoveAsync(Model);
                    else response.Failure(error: "Internal error while clearing contacts.");
                    if (response.IsSuccessful && States.Delete == State)
                    {
                        var contactsResponse = await _contactsService.ClearUnaligned();
                        if (contactsResponse.IsSuccessful)
                            Snackbar.Add("Contactos correctamente eliminados.", Severity.Info);
                        else
                            Snackbar.Add("Error al eliminar contactos.", Severity.Error);
                    }
                }
                _processing = false;
                Dialog.Close(DialogResult.Ok(response));
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }

        #region Validations
        private static IEnumerable<string> ValidateEmail(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                yield return "El campo es obligatorio.";
                yield break;
            }
            if (!Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                yield return "El campo no es válido.";
        }
        private static IEnumerable<string> ValidatePhone(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                yield return "El campo es obligatorio.";
                yield break;
            }
            if (Regex.IsMatch(input, "[a-zA-Z]+"))
                yield return "El campo no admite caracteres alfabéticos.";
            if (input.Any(ch => !char.IsLetterOrDigit(ch)) && Regex.IsMatch(input, @"[^+\-\s]+"))
                yield return "El campo no admite caracteres especiales más que \'+\', \'-\' y espacios en blanco.";
        }
        private static IEnumerable<string> ValidateName(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                yield return "El campo es obligatorio.";
                yield break;
            }
            if (Regex.IsMatch(input, "[0-9]"))
                yield return "El campo no admite caracteres numéricos.";
            if (input.Any(ch => !char.IsLetterOrDigit(ch) && ' ' != ch))
                yield return "El campo no admite caracteres especiales más que espacios en blanco.";
        }
        #endregion
    }
}
