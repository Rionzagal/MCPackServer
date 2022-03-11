using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCPackServer.Pages.UserManagementModule
{
    //public partial class UserManagementPage
    //{
    //    #region Dependency Injection
    //    [Inject]
    //    public IAuthorizationService _authorizationService { get; set; }
    //    [Inject]
    //    public IAuthenticationService _authenticationService { get; set; }
    //    [Inject]
    //    public NavigationManager _navigationManager { get; set; }
    //    #endregion

    //    #region Permission State
    //    public bool CanCreate, CanEdit, CanDelete = false;
    //    protected bool FormSuccess = false;
    //    #region Visible Flags
    //    public bool VisibleUserInformation = false;
    //    public bool VisibleUserEdit = false;
    //    public bool VisiblePasswordReset = false;
    //    #endregion
    //    #endregion

    //    #region MudBlazor Components
    //    public MudTable<AspNetUsers> UsersGrid;
    //    public MudForm EditUserForm;
    //    public MudForm ResetPasswordForm;
    //    public MudDialog EditUserDialog;
    //    public MudDialog ResetPasswordDialog;
    //    #endregion

    //    #region API Elements
    //    public DataManagerRequest UsersDm;
    //    public string UsersFilterString = null;
    //    public string UsersPassword = null;
    //    public string UsersPasswordConfirmation = null;
    //    #endregion

    //    #region Entities and Models
    //    public AspNetUsers SelectedUser;
    //    public UserInformation SelectedUserInfo;
    //    public AspNetUsers NewUser;
    //    #endregion

    //    protected override async Task OnInitializedAsync()
    //    {
    //        var AuthenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
    //        var user = AuthenticationState.User;

    //        try
    //        {
    //            CanCreate = (await _authorizationService.AuthorizeAsync(user, permission.Create)).Succeeded;
    //            CanEdit = (await _authorizationService.AuthorizeAsync(user, permission.Edit)).Succeeded;
    //            CanDelete = (await _authorizationService.AuthorizeAsync(user, permission.Delete)).Succeeded;
    //        }
    //        catch(Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //        }
    //    }

    //    private void ToggleUserInformation()
    //    {
    //        VisibleUserInformation = !VisibleUserInformation;
    //    }

    //    private void ToggleUserEdit()
    //    {
    //        VisibleUserEdit = !VisibleUserEdit;
    //    }

    //    private void ToggleUserPasswordReset()
    //    {
    //        VisiblePasswordReset = !VisiblePasswordReset;
    //    }

    //    private async Task<TableData<AspNetUsers>> ServerReload(TableState state)
    //    {
    //        UsersDm = new()
    //        {
    //            Select = new List<string>() { "Id", "UserName", "Email" },
    //            Take = state.PageSize,
    //            Skip = state.Page * state.PageSize,
    //            Sorted = SortDirection.None != state.SortDirection 
    //            ? new Sort() { Name = state.SortLabel, AscendingOrder = state.SortDirection == SortDirection.Ascending }
    //            : null,
    //            RequiresCounts = true
    //        };
    //        var response = await Http.PostAsJsonAsync("api/Users/Search", UsersDm);
    //        JObject JsonElement = JObject.Parse(await response.Content.ReadAsStringAsync());
    //        return new TableData<AspNetUsers>()
    //        {
    //            Items = JsonSerializer.Deserialize<IEnumerable<AspNetUsers>>(JsonElement.GetValue("Result").ToString()),
    //            TotalItems = int.Parse(JsonElement.GetValue("Count").ToString())
    //        };
    //    }

    //    private async Task OnSelectedRow(TableRowClickEventArgs<AspNetUsers> args)
    //    {
    //        var SelectedItemId = args.Item.Id;
    //        SelectedUser = await Http.GetFromJsonAsync<AspNetUsers>($"api/Users/GetIdentityUserById/{SelectedItemId}");
    //        SelectedUserInfo = await Http.GetFromJsonAsync<UserInformation>($"api/Users/GetUserInformationById/{SelectedItemId}");
    //        VisibleUserInformation = true;
    //    }

    //    private void GoToRegistration()
    //    {
    //        _navigationManager.NavigateTo("/UserManagement/RegisterUser");
    //    }

    //    private IEnumerable<string> PasswordStrength(string arg)
    //    {
    //        if (string.IsNullOrEmpty(arg))
    //        {
    //            yield return "PORFAVOR PROPORCIONE UNA CONTRASEÑA.";
    //            yield break;
    //        }
    //        if (8 > arg.Length) { yield return "LA CONTRASEÑA DEBE DE TENER AL MENOS 8 CARACTERES."; }
    //        if (!Regex.IsMatch(arg, @"[A-Z]")) { yield return "LA CONTRASEÑA DEBE DE TENER AL MENOS UNA LETRA MAYÚSCULA."; }
    //        if (!Regex.IsMatch(arg, @"[a-z]")) { yield return "LA CONTRASEÑA DEBE DE TENER AL MENOS UNA LETRA MINÚSCULA."; }
    //        if (!Regex.IsMatch(arg, @"[0-9]")) { yield return "LA CONTRASEÑA DEBE DE TENER AL MENOS UN NÚMERO."; }
    //        if (!arg.Any(ch => !char.IsLetterOrDigit(ch))) { yield return "LA CONTRASEÑA DEBE DE TENER AL MENOS UN CARACTER ESPECIAL."; }
    //    }

    //    private string PasswordMatch(string arg)
    //    {
    //        if (UsersPassword != arg)
    //            return "LAS CONTRASEÑAS NO COINCIDEN.";
    //        return null;
    //    }

    //    private async Task ResetUserPassword()
    //    {
    //        ResetPassword password = new()
    //        {
    //            Id = SelectedUser.Id,
    //            Password = UsersPassword,
    //            ConfirmPassword = UsersPasswordConfirmation
    //        };
    //        var response = await _authenticationService.ResetPassword(password);
    //        if (response.IsSuccessfulRegistration)
    //        {
    //            VisiblePasswordReset = false;
    //            VisibleUserEdit = false;
    //            VisibleUserInformation = false;
    //            ResetPasswordDialog.Close();
    //            await UsersGrid.ReloadServerData();
    //        }
    //    }

    //    private async Task EditUser()
    //    {
    //        if (FormSuccess)
    //        {
    //            EntityDBModel<UserInformation> updateInfoModel = new()
    //            {
    //                KeyColumn = "AspNetUserId",
    //                Key = SelectedUserInfo.AspNetUserId,
    //                Value = SelectedUserInfo
    //            };
    //            var UserInfoResponse = await Http.PostAsJsonAsync("api/Users/UpdateUserInfo", updateInfoModel);
    //            if (UserInfoResponse.IsSuccessStatusCode)
    //            {
    //                EntityDBModel<AspNetUsers> updateUsersModel = new()
    //                {
    //                    KeyColumn = "Id",
    //                    Key = SelectedUser.Id,
    //                    Value = SelectedUser
    //                };
    //                var response = await Http.PostAsJsonAsync("api/Users/Update", updateUsersModel);
    //                if (response.IsSuccessStatusCode)
    //                {
    //                    VisibleUserEdit = false;
    //                    VisibleUserInformation = false;
    //                    await UsersGrid.ReloadServerData();
    //                    EditUserDialog.Close();
    //                }
    //            }
    //        }
    //    }

    //    private async Task DeleteUser()
    //    {
    //        EntityDBModel<AspNetUsers> DeleteModel = new()
    //        {
    //            KeyColumn = "Id",
    //            Key = SelectedUser.Id,
    //            Value = SelectedUser
    //        };
    //        var response = await Http.PostAsJsonAsync("api/Users/Remove", DeleteModel);
    //        if (response.IsSuccessStatusCode)
    //        {
    //            SelectedUser = new();
    //            ToggleUserInformation();
    //            await UsersGrid.ReloadServerData();
    //        }
    //    }
    //}
}
