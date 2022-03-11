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
using System.Threading.Tasks;

namespace MCPackServer.Pages.RequisitionsModule
{
    public partial class RequisitionsDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public Requisitions Model { get; set; }
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
        private List<AspNetUsers> users = new();
        private List<Requisitions> ExistentRequisitions = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nueva requisición";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar requisición seleccionada";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar requisición seleccionada";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else //should not get to this option
            {
                Dialog.Cancel();
            }
            DataManagerRequest dm = new()
            {
                Take = 0
            };
            var items = await _requisitionsService.GetForGridAsync<Requisitions>(dm);
            if (null != items) ExistentRequisitions = items.ToList();
        }

        private async Task Submit()
        {
            _processing = true;
            ActionResponse<Requisitions> response = new();
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State) response = await _requisitionsService.AddAsync(Model);
                else if (States.Edit == State) response = await _requisitionsService.UpdateAsync(Model);
                else if (States.Delete == State) response = await _requisitionsService.RemoveAsync(Model);
                Dialog.Close(DialogResult.Ok(response));
            }
            else
            {
                if (!Form.IsValid)
                    Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }

        private async Task<IEnumerable<string>> UsersServerReload(string filter)
        {
            List<string> result = new();
            DataManagerRequest dm = new()
            {
                Where = new List<WhereFilter>
                {
                    new WhereFilter { Field = "UserName", Value = filter }
                }
            };
            var items = await _service.GetForGridAsync<AspNetUsers>(dm);
            if (null != items)
            {
                users = items.ToList();
                users.ForEach(x => result.Add(x.Id));
            }
            return result;
        }

        private string GetUserName(string Id)
        {
            string userName = string.Empty;
            if (!string.IsNullOrEmpty(Id))
            {
                var match = users.SingleOrDefault(u => u.Id == Id);
                if (null != match) userName = match.UserName;
            }
            return userName;
        }

        private IEnumerable<string> ValidateRequisitionsNumber(string number)
        {
            if (string.IsNullOrEmpty(number) || string.IsNullOrWhiteSpace(number))
            {
                yield return "Este campo es obligatorio.";
                yield break;
            }
            if (number.Any(ch => char.IsLetter(ch))) 
                yield return "El campo no admite caracteres alfabéticos.";
            if (number.Any(ch => !char.IsLetterOrDigit(ch))) 
                yield return "El campo no admite caracteres especiales.";
            if (ExistentRequisitions.Any(r => number.Equals(r.RequisitionNumber)))
                yield return "El valor insertado ya está en uso.";
        }
    }
}
