using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MCPackServer.Pages.ProvidersModule
{
    public partial class ProvidersDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public Providers Model { get; set; } = new();
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private MudForm Form = new();
        private List<Contacts> ProviderContacts = new();
        #endregion

        protected override void OnInitialized()
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
                Dialog?.Cancel();
            }
        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State)
                {
                    if (string.IsNullOrEmpty(Model.Website))
                        Model.Website = "N/A";
                    var response = await _service.AddAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Proveedor añadido con éxito.", Severity.Success);
                    else
                        foreach (var item in response.Errors)
                        {
                            Snackbar.Add(item, Severity.Error);
                        }
                }
                else if (States.Edit == State)
                {
                    var response = await _service.UpdateAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Proveedor editado con éxito.", Severity.Success);
                    else
                        foreach (var item in response.Errors)
                        {
                            Snackbar.Add(item, Severity.Error);
                        }
                }
                else if (States.Delete == State)
                {
                    var response = await _service.RemoveAsync(Model);
                    if (response.IsSuccessful)
                    {
                        Snackbar.Add("Proveedor añadido con éxito.", Severity.Success);
                        var _clearResponse = await _contactsService.ClearUnaligned();
                        if (_clearResponse.IsSuccessful)
                            Snackbar.Add("Contactos correctamente eliminados.", Severity.Info);
                        else
                            foreach (var item in _clearResponse.Errors)
                            {
                                Snackbar.Add(item, Severity.Error);
                            }
                    }
                    else
                        foreach (var item in response.Errors)
                        {
                            Snackbar.Add(item, Severity.Error);
                        }
                }
                Dialog?.Close();
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
