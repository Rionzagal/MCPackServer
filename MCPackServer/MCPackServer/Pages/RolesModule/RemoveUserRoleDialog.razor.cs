using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCPackServer.Pages.RolesModule
{
    public partial class RemoveUserRoleDialog
    {
        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public AspNetRoles Reference { get; set; }
        [Parameter]
        public AspNetUsers Model { get; set; }
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

            if (!string.IsNullOrEmpty(Reference.Id))
            {
                Title = $"Usuario de rol: {Model.UserName}";
                TitleIcon = Icons.Material.Filled.Visibility;
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
            AspNetUserRoles item = new() { UserId = Model.Id, RoleId = Reference.Id };
            await Form.Validate();
            if (Form.IsValid)
            {
                var response = await _service.RemoveAsync(item);
                _processing = false;
                Dialog.Close(DialogResult.Ok(response.IsSuccessful));
            }
            else
            {
                if (!Form.IsValid)
                    Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }
    }
}
