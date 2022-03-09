using Microsoft.AspNetCore.Authorization;

namespace MCPackServer.Utility.Security
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler() { }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (null == context.User) await Task.CompletedTask;
            var permissions = context.User.Claims.Where(x => "Permission" == x.Type &&
                                                               requirement.Permission == x.Value &&
                                                               "LOCAL AUTHORITY" == x.Issuer ||
                                                               "http://localhost:10836" == x.Issuer);
            if (permissions.Any())
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
        }
    }
}
