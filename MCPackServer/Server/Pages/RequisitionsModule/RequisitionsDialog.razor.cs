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
using System.Threading.Tasks;

namespace MCPackServer.Pages.RequisitionsModule
{
    public partial class RequisitionsDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public RequisitionsView? ModelView { get; set; }
        [Parameter]
        public string? UserId { get; set; }
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
        private List<AspNetUsers> users = new();
        private List<Requisitions> ExistentRequisitions = new();
        private Requisitions Model = new()
        {
            RequisitionNumber = "00001",
            IssuedDate = DateTime.Now,
            RequiredDate = DateTime.Now
        };
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (null == ModelView)
            {
                ExistentRequisitions = (await _service.GetForGridAsync<Requisitions>(new(), getAll: true))?.ToList()
                ?? new();
                if (null != ExistentRequisitions && ExistentRequisitions.Any())
                    Model.RequisitionNumber = (ExistentRequisitions.Max(r => int.Parse(r.RequisitionNumber)) + 1).ToString("d5");
                Model.UserId = UserId ?? string.Empty;
            }
            _ = await UsersServerReload(string.Empty);
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
                    var response = await _service.AddAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Requisición añadida con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                }
                else if (States.Edit == State)
                {
                    var response = await _service.UpdateAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Requisición editada con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                }
                else if (States.Delete == State)
                {
                    var response = await _service.RemoveAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Requisición eliminada con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                }
                Dialog?.Close();
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
                    new WhereFilter { Field = "UserName", Value = filter, Operator = Operators.Contains }
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
    }
}
