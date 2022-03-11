using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCPackServer.Pages.UserManagementModule
{
    //public partial class UserRegistrationPage
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
    //    protected string[] FormErrors = Array.Empty<string>();
    //    #region Visible Flags
    //    #endregion
    //    #endregion

    //    #region MudBlazor Components
    //    public MudForm NewUserForm;
    //    #endregion

    //    #region API Elements
    //    #endregion

    //    #region Entities and Models
    //    NewUserRegistration RegistrationModel;
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
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //        }

    //        RegistrationModel = new() { EmailConfirmed = false };
    //    }

    //    protected IEnumerable<string> PasswordStrength(string arg)
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

    //    protected string PasswordMatch(string arg)
    //    {
    //        if (RegistrationModel.Password != arg)
    //            return "LAS CONTRASEÑAS NO COINCIDEN.";
    //        return null;
    //    }

    //    private async Task RegisterUser()
    //    {
    //        var response = await _authenticationService.RegisterUser(RegistrationModel);
    //        if (response.IsSuccessfulRegistration) _navigationManager.NavigateTo("/UserManagement");
    //    }
    //}
}
