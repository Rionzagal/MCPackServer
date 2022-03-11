using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.RegularExpressions;

namespace MCPackServer.Pages.ProvidersModule
{
    public partial class ProvidersDialog
    {
        [Inject]
        public IJSRuntime _runtime { get; set; }
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public Providers Model { get; set; } = new();
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
        private List<Contacts> ProviderContacts = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nuevo proveedor";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar proveedor seleccionado";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar proveedor seleccionado";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else //should not get to this option
            {
                Title = null;
                TitleIcon = null;
                Disabled = true;
                ButtonColor = Color.Default;
                Dialog.Cancel();
            }
        }

        private async Task Submit()
        {
            _processing = true;
            ActionResponse<Providers> response = new();
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State) response = await _service.AddAsync(Model);
                else if (States.Edit == State) response = await _service.UpdateAsync(Model);
                else if (States.Delete == State)
                {
                    response = await _service.RemoveAsync(Model);
                    if (response.IsSuccessful)
                    {
                        var _clearResponse = await _contactsService.ClearUnaligned();
                        if (_clearResponse.IsSuccessful)
                            Snackbar.Add("Contactos correctamente eliminados.", Severity.Info);
                        else
                            Snackbar.Add("Error al eliminar contactos.", Severity.Error);
                    }
                }
                Dialog.Close(DialogResult.Ok(response));
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }

        private async Task<TableData<Contacts>> ProviderContactsLoad(TableState state, object providerId)
        {
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.Page * state.PageSize
            };
            var items = await _providersService.GetContacts(providerId, request);
            int? count = await _providersService.CountContacts(providerId, request);
            return new TableData<Contacts>()
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }

        #region Validations
        private  IEnumerable<string> ValidateEmail(string input)
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

        //async private void Enter() => await _runtime.InvokeVoidAsync("EnterToTab");
    }
}
