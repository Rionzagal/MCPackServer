using MCPackServer.Constants;
using MCPackServer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace MCPackServer.Pages.UserManagementModule
{
    public partial class UserDetails
    {
        #region Dependency injection and parameters
        [Parameter]
        public string? Id { get; set; }
        [Inject]
        public NavigationManager navManager { get; set; }
        [Inject]
        public IAuthenticationService authenticationService { get; set; }
        #endregion

        #region MudBlazor components

        #endregion

        #region Models, permissions and flags
        #region Models
        private AspNetUsers CurrentUser = new();
        private PersonInformation UserPersonInformation = new();
        private UserPersonalInformationView CurrentUserPersonalInfoView = new();
        private List<HistoryView> UserHistoryActions = new();
        private HashSet<string> UserPermissions = new();
        private List<AspNetRoles> UserRoles = new();
        #endregion
        #region Permissions
        private bool CanEditUser = false;
        private bool CanDeleteUser = false;
        private bool CanReadPermissions = false;
        private bool CanEditPermissions = false;
        private bool CanReadRoles = false;
        private bool CanEditRoles = false;
        private bool CanViewHistory = false;
        #endregion
        private bool IsPersonalProfile = false;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            var user = (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;
            try
            {
                CanEditUser = (await _authorizationService.AuthorizeAsync(user, Permissions.Users.Edit)).Succeeded;
                CanReadPermissions = (await _authorizationService.AuthorizeAsync(user, Permissions.RoleClaims.View)).Succeeded;
                CanEditPermissions = (await _authorizationService.AuthorizeAsync(user, Permissions.RoleClaims.Edit)).Succeeded;
                CanEditRoles = (await _authorizationService.AuthorizeAsync(user, Permissions.Roles.Edit)).Succeeded;
                CanViewHistory = (await _authorizationService.AuthorizeAsync(user, Permissions.History.View)).Succeeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            var user = (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;
            string userId = user.FindFirst(c => c.Type == "sub")?.Value ?? string.Empty;
            IsPersonalProfile = (Id == userId || string.IsNullOrEmpty(Id));

            if (IsPersonalProfile)
                await LoadUserInfo(userId);
            else
            {
                if (string.IsNullOrEmpty(Id))
                    navManager.NavigateTo("Profile");
                else
                    await LoadUserInfo(Id);
            }
        }

        private async Task LoadUserInfo(string userId)
        {
            CurrentUser = await _service.GetByKeyAsync<AspNetUsers>(userId);
            CurrentUserPersonalInfoView = await _service.GetByKeyAsync<UserPersonalInformationView>(userId);
            UserPersonInformation = await _service.GetByKeyAsync<PersonInformation>(userId, nameof(PersonInformation.AspNetUserId));
            if (CanViewHistory)
                Console.WriteLine("User can view history");
            if (CanReadRoles)
                Console.WriteLine("User can read roles");
            if (CanReadPermissions)
                Console.WriteLine("User can read permissions");
        }
    }
}
