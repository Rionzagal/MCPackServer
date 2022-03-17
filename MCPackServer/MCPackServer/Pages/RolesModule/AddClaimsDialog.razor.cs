using MCPackServer.Constants;
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
    public partial class AddClaimsDialog
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
        private List<AspNetRoleClaims> PermissionList = new();
        private HashSet<AspNetRoleClaims> SelectedPermissions = new();
        #endregion

        private MudTable<AspNetRoleClaims> PermissionsTable;
        private TableGroupDefinition<AspNetRoleClaims> PermissionGrouping = new();

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(Reference.Id)) //representing an Add dialog
            {
                Title = $"Añadir nuevo permiso al rol: {Reference.Name}";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else //should not get to this option
            {
                Dialog.Cancel();
            }
            #region Get unassociated users
            foreach (var claim in PermissionModules.GetAllPermissionsMenus())
            {
                foreach (var permission in PermissionModules.GeneratePermissionsForMenu(claim))
                {
                    if (!Reference.AspNetRoleClaims.Any(c => claim == c.ClaimValue))
                        PermissionList.Add(new AspNetRoleClaims()
                        {
                            ClaimType = "Permission",
                            RoleId = Reference.Id,
                            ClaimValue = permission
                        });
                }
            }
            foreach (var claim in PermissionModules.GetAllPermissionsModules())
            {
                foreach (var permission in PermissionModules.GeneratePermissionsForModule(claim))
                {
                    if (!Reference.AspNetRoleClaims.Any(c => claim == c.ClaimValue))
                        PermissionList.Add(new AspNetRoleClaims()
                        {
                            ClaimType = "Permission",
                            RoleId = Reference.Id,
                            ClaimValue = permission
                        });
                }
            }
            foreach (var claim in PermissionModules.GetSpecialPermissions())
            {
                if (!Reference.AspNetRoleClaims.Any(c => claim == c.ClaimValue))
                {
                    PermissionList.Add(new AspNetRoleClaims()
                    {
                        ClaimType = "Permission",
                        RoleId = Reference.Id,
                        ClaimValue = claim
                    });
                }
            }
            PermissionGrouping = new()
            {
                GroupName = "Módulo",
                Indentation = false,
                Expandable = true,
                IsInitiallyExpanded = false,
                Selector = (c) =>
                {
                    string result = "N/A";
                    if (c.ClaimValue.Contains('.'))
                    {
                        result = c.ClaimValue.Split('.')[1];
                    }
                    return result;
                }
            };
            #endregion
        }

        private async Task Submit()
        {
            _processing = true;
            List<AspNetRoleClaims> resultElements = new();
            foreach (var item in SelectedPermissions)
            {
                var response = await _service.AddAsync(item);
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
