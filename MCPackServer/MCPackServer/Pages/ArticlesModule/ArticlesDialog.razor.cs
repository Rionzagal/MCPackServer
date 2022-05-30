using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MCPackServer.Pages.ArticlesModule
{
    public partial class ArticlesDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public PurchaseArticles Model { get; set; }
        [Parameter]
        public string GroupCode { get; set; }
        [Parameter]
        public string FamilyCode { get; set; }
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
                Title = "Añadir nuevo artículo de compra";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar artículo seleccionado";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar artículo seleccionado";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else //should not get to this option
            {
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
                Dialog.Close(DialogResult.Ok(JsonConvert.DeserializeObject<ActionResponse<PurchaseArticles>>(response)));
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }

        private static IEnumerable<string> ValidateCode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                yield return "Ingrese código de artículo.";
                yield break;
            }
            if (8 < input.Length)
                yield return "El código no debe contener más de 8 caracteres.";
            if (Regex.IsMatch(input, "[a-z]"))
                yield return "El código no debe contener letras minúsculas.";
            if (input.Any(ch => !char.IsLetterOrDigit(ch)))
                yield return "El código no debe contener caracteres especiales.";
        }
    }
}
