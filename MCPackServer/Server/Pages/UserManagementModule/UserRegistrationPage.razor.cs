using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCPackServer.Pages.UserManagementModule
{
    public partial class UserRegistrationPage
    {
        #region Dependency Injection
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        #endregion

        #region Permission State
        private bool FormSuccess = false;
        private string[] FormErrors = Array.Empty<string>();
        #endregion

        #region MudBlazor Components
        private MudForm NewUserForm = new();
        #endregion

        #region Entities and Models
        private RegistrationModel RegistrationModel = new();
        private List<AspNetRoles> Roles = new();
        private string UserRoleId = string.Empty;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            var AuthenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = AuthenticationState.User;

            RegistrationModel = new() 
            {
                EmailConfirmed = false, 
                BirthDate = DateTime.Today,
                Email = string.Empty,
                UserName = string.Empty,
                FirstName = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                MiddleName = string.Empty,
                FatherSurname = string.Empty,
                MotherSurname = string.Empty,
                Gender = string.Empty
            };
        }

        private void ReturnToGrid() => _navigationManager.NavigateTo("Users");

        private async Task Submit()
        {
            var existentUserByEmail = await _userManager.FindByEmailAsync(RegistrationModel.Email);
            var existentUserByUserName = await _userManager.FindByNameAsync(RegistrationModel.UserName);
            if (FormSuccess && null == existentUserByEmail && null == existentUserByUserName)
            {
                List<string> Errors = new();
                var user = new IdentityUser()
                {
                    UserName = RegistrationModel.UserName,
                    Email = RegistrationModel.Email,
                    EmailConfirmed = RegistrationModel.EmailConfirmed
                };
                var registrationResult = await _userManager.CreateAsync(user, RegistrationModel.Password);
                if (registrationResult.Succeeded)
                {
                    PersonInformation personInformation = new()
                    {
                        AspNetUserId = user.Id,
                        FirstName = RegistrationModel.FirstName,
                        MiddleName = RegistrationModel.MiddleName,
                        FatherSurname = RegistrationModel.FatherSurname,
                        MotherSurname = RegistrationModel.MotherSurname,
                        BirthDate = RegistrationModel.BirthDate,
                        Gender = RegistrationModel.Gender
                    };
                    try
                    {
                        await _service.AddAsync(personInformation);
                        Snackbar.Add("Usuario registrado con éxito.", Severity.Success);
                    }
                    catch (Exception ex)
                    {
                        Errors.Add(ex.ToString());
                        await _userManager.DeleteAsync(user);
                        Snackbar.Add("Error al añadir información de usuario.", Severity.Error);
                    }
                    if (!string.IsNullOrEmpty(UserRoleId))
                    {
                        AspNetUserRoles UserRole = new() { UserId = user.Id, RoleId = UserRoleId };
                        var roleResponse = await _service.AddAsync(UserRole);
                        if (roleResponse.IsSuccessful) Snackbar.Add("Usuario asignado a rol exitosamente.", Severity.Success);
                        else
                            roleResponse.Errors.ForEach(e =>
                            {
                                Snackbar.Add($"Error: {e}", Severity.Error);
                                Console.WriteLine(e);
                            });
                    }
                }
                else
                {
                    Errors = registrationResult.Errors.Select(e => e.Description).ToList();
                    Errors.ForEach(item =>
                    {
                        Snackbar.Add($"Error: {item}", Severity.Error);
                    });
                }
            }
            else if (null != existentUserByEmail) Snackbar.Add("Usuario ya existente. Ingrese otro correo electrónico.", Severity.Warning);
            else if (null != existentUserByUserName) Snackbar.Add("Usuario ya existente.Ingrese otro nombre de usuario.", Severity.Warning);
            else Snackbar.Add("Operación inválida. Revise si hay errores en la forma.", Severity.Warning);
            ReturnToGrid();
        }

        private async Task<IEnumerable<string>> RolesServerReload(string filter)
        {
            List<string> roleNames = new();
            DataManagerRequest request = new()
            {
                Where = new List<WhereFilter>()
                {
                    new WhereFilter { Field = nameof(AspNetRoles.Name), Value = filter }
                }
            };
            var response = await _service.GetForGridAsync<AspNetRoles>(request);
            if (response != null)
            {
                Roles = response.ToList();
                Roles.ForEach(r => roleNames.Add(r.Id));
            }
            return roleNames;
        }

        private string GetRoleName(string Id)
        {
            string name = "";
            if (!string.IsNullOrEmpty(Id))
            {
                var match = Roles.SingleOrDefault(r => Id == r.Id);
                if (match != null) name = match.Name;
            }
            return name;
        }

        private static IEnumerable<string> PasswordStrength(string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                yield return "Este campo es obligatorio.";
                yield break;
            }
            if (8 > arg.Length) { yield return "La contraseña debe de tener al menos 8 caracteres."; }
            if (!Regex.IsMatch(arg, @"[A-Z]")) { yield return "La contraseña debe tener al menos una letra mayúscula."; }
            if (!Regex.IsMatch(arg, @"[a-z]")) { yield return "La contraseña debe tener al menos una letra minúscula."; }
            if (!Regex.IsMatch(arg, @"[0-9]")) { yield return "La contraseña debe tener al menos un número."; }
            if (!arg.Any(ch => !char.IsLetterOrDigit(ch))) { yield return "La contraseña debe tener al menos un caracter especial."; }
        }

        private string PasswordMatch(string arg)
        {
            if (RegistrationModel.Password != arg)
                return "Las contraseñas no coinciden.";
            return null;
        }

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
            if (Regex.IsMatch(input, "[a-zA-Z]+"))
                yield return "El campo no admite caracteres alfabéticos.";
            if (input.Any(ch => !char.IsLetterOrDigit(ch)) && Regex.IsMatch(input, @"[^+\-\s]+"))
                yield return "El campo no admite caracteres especiales más que \'+\', \'-\' y espacios en blanco.";
        }
    }
}
