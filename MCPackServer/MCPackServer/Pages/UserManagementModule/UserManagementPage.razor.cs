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
    public partial class UserManagementPage
    {
        #region Dependency Injection
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        #endregion

        #region Permission State
        public bool CanCreate, CanEdit, CanDelete = false;
        protected bool FormSuccess = false;
        #region Visible Flags
        public bool VisibleUserInformation = false;
        public bool VisibleUserEdit = false;
        public bool VisiblePasswordReset = false;
        #endregion
        #endregion

        #region MudBlazor Components
        public MudTable<UserInformationView> UsrersTable = new();
        public MudForm EditUserForm;
        public MudForm ResetPasswordForm;
        public MudDialog EditUserDialog;
        public MudDialog ResetPasswordDialog;
        private MudMessageBox DeleteMessage = new();
        #endregion

        #region Search filters
        private string IdFilter = string.Empty;
        private string ShortNameFilter = string.Empty;
        private string UserNameFilter = string.Empty;
        private string EmailFilter = string.Empty;
        private string GenderFilter = string.Empty;
        private string StatusFilter = string.Empty;
        #endregion

        #region Entities and Models
        public AspNetUsers SelectedUser;
        public UserInformation SelectedUserInfo;
        public AspNetUsers NewUser;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            var AuthenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = AuthenticationState.User;
            try
            {
                CanCreate = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Users.Create)).Succeeded;
                CanEdit = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Users.Edit)).Succeeded;
                CanDelete = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.Users.Delete)).Succeeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<TableData<UserInformationView>> UsersServerReload(TableState state)
        {
            string status = string.Empty;
            if ("Activo" == StatusFilter) status = "1";
            else if ("Inactivo" == StatusFilter) status = "0";
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.Page * state.PageSize,
                Where = new List<WhereFilter>()
                {
                    new WhereFilter() { Field = nameof(UserInformationView.Id), Value = IdFilter },
                    new WhereFilter() { Field = nameof(UserInformationView.ShortName), Value = ShortNameFilter },
                    new WhereFilter() { Field = nameof(UserInformationView.UserName), Value = UserNameFilter },
                    new WhereFilter() { Field = nameof(UserInformationView.Email), Value = EmailFilter },
                    new WhereFilter() { Field = nameof(UserInformationView.Gender), Value = GenderFilter },
                    new WhereFilter() { Field = nameof(UserInformationView.Active), Value = status}
                }
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var response = await _service.GetForGridAsync<UserInformationView>(request, field, order);
            int? count = await _service.GetTotalCountAsync<UserInformationView>(request);
            return new TableData<UserInformationView>()
            {
                Items = response ?? new List<UserInformationView>(),
                TotalItems = count ?? 0
            };
        }

        private async Task OnSelectedRow(TableRowClickEventArgs<UserInformationView> args)
        {
            var selectedId = args.Item.Id;
            SelectedUser = await _usersService.GetByKeyAsync<AspNetUsers>(selectedId);
            SelectedUserInfo = await _service.GetByKeyAsync<UserInformation>(args.Item.Id, "AspNetUserId");
            VisibleUserInformation = true;
        }

        private void ClearFilters() =>
            IdFilter = EmailFilter = UserNameFilter = 
            GenderFilter = ShortNameFilter = StatusFilter = string.Empty;

        private void GoToRegistration() => _navigationManager.NavigateTo("/Users/Register");

        private void GoToUserEdit() => _navigationManager.NavigateTo($"Users/Edit/{SelectedUser.Id}");

        private async Task DeleteUser()
        {
            bool? result = await DeleteMessage.Show();
            if (result.HasValue)
            {
                var user = await _userManager.FindByIdAsync(SelectedUser.Id);
                var response = await _userManager.DeleteAsync(user);
                if (null != response)
                {
                    List<string> errors = new();
                    if (null != response.Errors && response.Errors.Any())
                        errors = response.Errors.Select(e => e.Description).ToList();
                    if (response.Succeeded) Snackbar.Add("Usuario eliminado exitosamente.", Severity.Info);
                    else
                        errors.ForEach(item =>
                        {
                            Snackbar.Add($"Error: {item}", Severity.Error);
                            Console.WriteLine(item);
                        });
                }
            }
        }
    }
}
