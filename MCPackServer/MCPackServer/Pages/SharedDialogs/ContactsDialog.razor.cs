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
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public Contacts Model { get; set; } = new();
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
            string response = string.Empty;
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State)
                    response = JsonConvert.SerializeObject(await _service.AddAsync(Model));
                else if (States.Edit == State)
                    response = JsonConvert.SerializeObject(await _service.UpdateAsync(Model));
                else if (States.Delete == State)
                    response = JsonConvert.SerializeObject(await _service.RemoveAsync(Model));
                _processing = false;
                Dialog.Close(DialogResult.Ok(JsonConvert.DeserializeObject<ActionResponse<Contacts>>(response)));
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
