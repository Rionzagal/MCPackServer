using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
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
    public partial class RoleManagementPage
    {
    //    #region Dependency Injection
    //    [Inject]
    //    public NavigationManager _navigationManager { get; set; }
    //    #endregion

    //    #region Permission State
    //    #region Permissions
    //    public bool CanCreate, CanEdit, CanDelete = false;
    //    private bool VisibleRoleInformation = false;
    //    #endregion
    //    #endregion

    //    #region MudBlazor Components
    //    #region Tables
    //    private MudTable<AspNetRoles> RolesTable;
    //    private MudTable<AspNetRoleClaims> ClaimsTable;
    //    private MudTable<AspNetUserRoles> UsersTable;
    //    private TableGroupDefinition<AspNetRoleClaims> ClaimsGroupDefinition = new();
    //    #endregion
    //    #region Tabs
    //    private MudTabs RolesInformationTabs;
    //    #endregion
    //    #region Dialogs
    //    DialogParameters Parameters;
    //    #endregion
    //    #endregion

    //    #region API Elements
    //    private DataManagerRequest RolesDm;
    //    #endregion

    //    #region Entities and Models
    //    AspNetRoles SelectedRole = new();
    //    List<AspNetRoles> RolesTableItems = new();
    //    #endregion

    //    protected override async Task OnInitializedAsync()
    //    {
    //        var AuthenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
    //        var user = AuthenticationState.User;
    //    }

    //    #region RolesTable methods
    //    private async Task<TableData<AspNetRoles>> RolesServerReload(TableState state)
    //    {
    //        RolesDm = new()
    //        {
    //            Take = state.PageSize,
    //            Skip = state.Page * state.PageSize
    //        };
    //        string field = state.SortLabel ?? "Id";
    //        string order = SortDirection.Ascending == state.SortDirection ? "ASC" : "DESC";
    //        var response = await _service.GetForGridAsync<AspNetRoles>(RolesDm, field, order);
    //        if (null != response) RolesTableItems = response.ToList();
    //        int? count = await _service.GetTotalCountAsync<AspNetRoles>(RolesDm);
    //        return new TableData<AspNetRoles>()
    //        {
    //            Items = RolesTableItems,
    //            TotalItems = count ?? 0
    //        };
    //    }
    //    private async Task OnSelectedRole(TableRowClickEventArgs<AspNetRoles> args)
    //    {
    //        SelectedRole = args.Item;
    //        SelectedRole.AspNetUserRoles = await UsersServerReload(args.Item.Id);
    //        SelectedRole.AspNetRoleClaims = await ClaimsServerReload(args.Item.Id);
    //        VisibleRoleInformation = true;
    //    }
    //    #endregion
    //    #region CRUD methods
    //    private async Task AddRole()
    //    {
    //        Parameters = new()
    //        {
    //            ["State"] = RolesDialog.States.Add,
    //            ["Model"] = new AspNetRoles()
    //        };
    //        var dialog = Dialogs.Show<RolesDialog>("Añadir nuevo rol de usuario", Parameters);
    //        var result = await dialog.Result;
    //        if (!result.Cancelled)
    //        {
    //            JObject element = JObject.Parse(result.Data.ToString());
    //            if (bool.Parse(element.GetValue("Success").ToString()))
    //            {
    //                VisibleRoleInformation = false;
    //                SelectedRole = new();
    //                Snackbar.Add("Rol de usuario añadido con éxito.", Severity.Success);
    //            }
    //            else
    //                Snackbar.Add("Error al añadir rol de usuario.", Severity.Error);
    //            await RolesTable.ReloadServerData();
    //        }
    //    }
    //    private async Task DeleteRole(AspNetRoles model)
    //    {
    //        Parameters = new()
    //        {
    //            ["State"] = RolesDialog.States.Delete,
    //            ["Model"] = model
    //        };
    //        var dialog = Dialogs.Show<RolesDialog>("Eliminar rol de usuario", Parameters);
    //        var result = await dialog.Result;
    //        if (!result.Cancelled)
    //        {
    //            JObject element = JObject.Parse(result.Data.ToString());
    //            if (bool.Parse(element.GetValue("Success").ToString()))
    //            {
    //                VisibleRoleInformation = false;
    //                SelectedRole = new();
    //                Snackbar.Add("Rol de usuario eliminado con éxito.", Severity.Info);
    //            }
    //            else
    //                Snackbar.Add("Error al eliminar rol de usuario.", Severity.Error);
    //            await RolesTable.ReloadServerData();
    //        }
    //    }
    //    #endregion

    //    #region UsersTable methods
    //    private async Task<ICollection<AspNetUserRoles>> UsersServerReload(string roleId)
    //    {
    //        List<AspNetUserRoles> result = await _service.GetForGridAsync<AspNetUserRoles>
    //            ($"api/Roles/GetAssociatedUsers/{roleId}");
    //        return result;
    //    }
    //    private async Task AddUser()
    //    {
    //        Parameters = new() { ["Reference"] = SelectedRole };
    //        var dialog = Dialogs.Show<AddUserRoleDialog>("Añadir usuarios a rol", Parameters);
    //        var result = await dialog.Result;
    //        if (!result.Cancelled)
    //        {
    //            var ResultElements = JsonSerializer.Deserialize<List<AspNetUserRoles>>
    //                (result.Data.ToString());
    //            Snackbar.Add($"{ResultElements.Count} usuarios añadidos a rol", Severity.Info);
    //        }
    //        SelectedRole.AspNetUserRoles = await UsersServerReload(SelectedRole.Id);
    //    }
    //    private async Task RemoveUser(AspNetUsers user)
    //    {
    //        Parameters = new()
    //        {
    //            ["Reference"] = SelectedRole,
    //            ["Model"] = user
    //        };
    //        var dialog = Dialogs.Show<RemoveUserRoleDialog>("Ver usuario en rol", Parameters);
    //        var result = await dialog.Result;
    //        if (!result.Cancelled)
    //        {
    //            if (bool.Parse(result.Data.ToString()))
    //                Snackbar.Add("Usuario removido con éxito de rol", Severity.Info);
    //            else
    //                Snackbar.Add("Error al remover usuario de rol", Severity.Error);
    //            SelectedRole.AspNetUserRoles = await UsersServerReload(SelectedRole.Id);
    //        }
    //    }
    //    #endregion

    //    #region ClaimsTable methods
    //    private async Task<ICollection<AspNetRoleClaims>> ClaimsServerReload(string roleId)
    //    {
    //        List<AspNetRoleClaims> result = new();
    //        DataManagerRequest request = new()
    //        {
    //            Take = 0,
    //            RequiresCounts = false,
    //            Where = new List<WhereFilter>()
    //            {
    //                new WhereFilter { Field = "RoleId", Value = roleId }
    //            }
    //        };
    //        var response = await Http.PostAsJsonAsync("api/RoleClaims/Search", request);
    //        if (response.IsSuccessStatusCode)
    //        {
    //            JObject element = JObject.Parse(await response.Content.ReadAsStringAsync());
    //            result = JsonSerializer.Deserialize<List<AspNetRoleClaims>>
    //                (element.GetValue("Result").ToString());
    //            ClaimsGroupDefinition = new()
    //            {
    //                GroupName = "Módulo",
    //                Indentation = false,
    //                Expandable = true,
    //                IsInitiallyExpanded = false,
    //                Selector = (c) => c.ClaimValue.Split('.')[1]
    //            };
    //        }
    //        return result;
    //    }
    //    private async Task AddPermissions()
    //    {
    //        Parameters = new() { ["Reference"] = SelectedRole };
    //        var dialog = Dialogs.Show<AddClaimsDialog>("Añadir permisos al rol", Parameters);
    //        var result = await dialog.Result;
    //        if (!result.Cancelled)
    //        {
    //            var ResultElements = JsonSerializer.Deserialize<List<AspNetRoleClaims>>
    //                (result.Data.ToString());
    //            if (ResultElements.Any())
    //                Snackbar.Add($"{ResultElements.Count} permisos añadidos a rol", Severity.Info);
    //            else
    //                Snackbar.Add("Error al añadir permisos al rol", Severity.Error);
    //            SelectedRole.AspNetRoleClaims = await ClaimsServerReload(SelectedRole.Id);
    //        }
    //    }

    //    private async Task DeletePermissions(IEnumerable<AspNetRoleClaims> permissions)
    //    {
    //        bool? delete = await Dialogs.ShowMessageBox(
    //            "Advertencia",
    //            $"Se eliminarán {permissions.Count()} permisos de rol",
    //            yesText: "Confirmar",
    //            cancelText: "Cancelar");
    //        if (delete.HasValue)
    //        {
    //            foreach (var item in permissions)
    //            {
    //                EntityDBModel<AspNetRoleClaims> payload = new()
    //                {
    //                    Action = "Remove",
    //                    KeyColumn = nameof(AspNetRoleClaims.Id),
    //                    Key = item.Id,
    //                    Value = item
    //                };
    //                var response = await Http.PostAsJsonAsync("api/RoleClaims/Remove", payload);
    //                if (response.IsSuccessStatusCode)
    //                    Snackbar.Add("Permiso removido con éxito", Severity.Info);
    //                else
    //                    Snackbar.Add("Error al remover permiso", Severity.Error);
    //            }
    //            SelectedRole.AspNetRoleClaims = await ClaimsServerReload(SelectedRole.Id);
    //        }
    //    }
    //    #endregion
    }
}
