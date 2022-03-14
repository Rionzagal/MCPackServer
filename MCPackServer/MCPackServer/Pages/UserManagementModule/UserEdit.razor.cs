using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using System.Text.RegularExpressions;

namespace MCPackServer.Pages.UserManagementModule
{
    public partial class UserEdit
    {
        #region Parameters and Dependency Injection
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public NavigationManager _navigationManager { get; set; }
        #endregion

        #region MudItems
        private MudForm AspNetUsersForm = new();
        private MudForm PasswordResetForm = new();
        private MudForm UserInfoForm = new();
        #endregion

        #region Entities
        private IdentityUser CurrentUser = new();
        private IdentityUser EditUserModel = new();

        private UserInformation CurrentUserInformation = new();
        private UserInformation EditUserInfoModel = new();

        private string NewPasswordText = string.Empty;
        private string NewPasswordConfirm = string.Empty;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            try
            {
                CurrentUser = await _userManager.FindByIdAsync(Id);
                CurrentUserInformation = await _service.GetByKeyAsync<UserInformation>(Id, nameof(UserInformation.AspNetUserId));
                ResetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ReturnToGrid() => _navigationManager.NavigateTo("Users");

        private void ResetIdentityUser()
        {
            EditUserModel = new()
            {
                Id = CurrentUser.Id,
                UserName = CurrentUser.UserName,
                Email = CurrentUser.Email,
                PhoneNumber = CurrentUser.PhoneNumber,
                EmailConfirmed = CurrentUser.EmailConfirmed,
            };
        }

        private void ResetUserInformation()
        {
            var properties = typeof(UserInformation).GetProperties()
                .Where(x => x.GetAccessors()[0].IsFinal || !x.GetAccessors()[0].IsVirtual)
                .ToList();
            foreach (var property in properties)
            {
                var value = property.GetValue(CurrentUserInformation);
                property.SetValue(EditUserInfoModel, value);
            }
        }

        private void ResetPasswordFields() => NewPasswordText = NewPasswordConfirm = string.Empty;

        private void ResetAll()
        {
            ResetUserInformation();
            ResetIdentityUser();
            ResetPasswordFields();
        }

        private async Task SubmitIdentityUser()
        {
            await AspNetUsersForm.Validate();
            if (AspNetUsersForm.IsValid)
            {
                var response = await _userManager.UpdateAsync(EditUserModel);
                if (null != response)
                {
                    List<string> errors = new();
                    if (null != response.Errors && response.Errors.Any()) 
                        errors = response.Errors.Select(e => e.Description).ToList();
                    if (response.Succeeded) Snackbar.Add("Usuario editado exitosamente.", Severity.Success);
                    else
                        errors.ForEach(item =>
                        {
                            Snackbar.Add($"Error: {item}", Severity.Error);
                            Console.WriteLine(item);
                        });
                }
            }
            else
                Snackbar.Add("Operación no válida. Revise si hay un error en la forma.", Severity.Warning);
        }

        private async Task SubmitPassword()
        {
            await PasswordResetForm.Validate();
            if (PasswordResetForm.IsValid)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(CurrentUser);
                var response = await _userManager.ResetPasswordAsync(CurrentUser, code, NewPasswordText);
                if (null != response)
                {
                    List<string> errors = new();
                    if (null != response.Errors && response.Errors.Any())
                        errors = response.Errors.Select(e => e.Description).ToList();
                    if (response.Succeeded) Snackbar.Add("Contraseña reestablecida exitosamente.", Severity.Success);
                    else
                        errors.ForEach(item =>
                        {
                            Snackbar.Add($"Error: {item}", Severity.Error);
                            Console.WriteLine(item);
                        });
                }
            }
            else
                Snackbar.Add("Operación no válida. Revise si hay un error en la forma.", Severity.Warning);
        }

        private async Task SubmitUserInfo()
        {
            await UserInfoForm.Validate();
            if (UserInfoForm.IsValid)
            {
                var response = await _service.UpdateAsync(EditUserInfoModel);
                if (null != response)
                {
                    List<string> Errors = new();
                    if (null != response.Errors && response.Errors.Any())
                        Errors = response.Errors;
                    if (response.IsSuccessful) Snackbar.Add("Usuario editato exitosamente.", Severity.Success);
                    else
                        Errors.ForEach(item =>
                        {
                            Snackbar.Add($"Error: {item}", Severity.Error);
                            Console.WriteLine(item);
                        });
                }
            }
            else
                Snackbar.Add("Operación no válida. Revise si hay un error en la forma.", Severity.Warning);
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
            if (NewPasswordText != arg)
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
