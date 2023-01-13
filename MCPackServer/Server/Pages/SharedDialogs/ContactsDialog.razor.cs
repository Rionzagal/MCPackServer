using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCPackServer.Pages.SharedDialogs
{
    public partial class ContactsDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public Contacts Model { get; set; } = new();
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
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir contacto cliente";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar contacto seleccionado";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar contacto seleccionado";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else //should not get to this option
            {
                Disabled = true;
                ButtonColor = Color.Default;
                Dialog?.Cancel();
            }
        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            if (Form.IsValid)
            {
                bool success = false;
                if (States.Add == State)
                {
                    var response = await _service.AddAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Contacto añadido con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    success = response.IsSuccessful;
                }
                else if (States.Edit == State)
                {
                    var response = await _service.UpdateAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Contacto editado con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    success = response.IsSuccessful;
                }
                else if (States.Delete == State)
                {
                    var response = await _service.RemoveAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Contacto eliminado con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    success = response.IsSuccessful;
                }
                _processing = false;
                Dialog?.Close(DialogResult.Ok(success ? Model.Id : 0));
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
            if (!string.IsNullOrEmpty(input) && !Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                yield return "El campo no es válido. Favor de poner dominio.";
        }
        private static IEnumerable<string> ValidatePhone(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (Regex.IsMatch(input, "[a-zA-Z]+"))
                    yield return "El campo no admite caracteres alfabéticos.";
                if (input.Any(ch => !char.IsLetterOrDigit(ch)) && Regex.IsMatch(input, @"[^+\-\s]+"))
                    yield return "El campo no admite caracteres especiales más que \'+\', \'-\' y espacios en blanco.";
            }
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
