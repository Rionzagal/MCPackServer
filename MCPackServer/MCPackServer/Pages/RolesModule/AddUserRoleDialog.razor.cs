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

namespace MCPackServer.Pages.RolesModule
{
    public partial class AddUserRoleDialog
    {

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public AspNetRoles Reference { get; set; }
        #endregion

        #region Dialog variables
        private string Title;
        private string TitleIcon;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private string Request;
        private AspNetRoles CurrentRole = new();
        private List<AspNetUsers> AvailableUsers = new();
        private HashSet<AspNetUsers> SelectedUsers = new();
        #endregion

        private MudTable<AspNetUsers> UsersTable;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(Reference.Id)) //representing an Add dialog
            {
                Title = "Añadir nuevo usuario al rol";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else //should not get to this option
            {
                Title = null;
                TitleIcon = null;
                Disabled = true;
                ButtonColor = Color.Default;
                Dialog.Cancel();
            }
            #region Get unassociated users
            var currentRole = await _roleManager.FindByIdAsync(Reference.Id);
            #endregion
        }

        private async Task Submit()
        {
            _processing = true;
            List<AspNetUserRoles> resultElements = new();
            foreach (var item in SelectedUsers)
            {
                AspNetUserRoles UserRole = new() { UserId = item.Id, RoleId = Reference.Id };
                var response = await _service.AddAsync(UserRole);
                if (response.IsSuccessful)
                {
                    resultElements.Add(response.Value);
                }
            }
            _processing = false;
            Dialog.Close(DialogResult.Ok(JsonSerializer.Serialize(resultElements)));
        }
    }
}
