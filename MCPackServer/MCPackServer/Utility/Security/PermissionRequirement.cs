using Microsoft.AspNetCore.Authorization;

namespace MCPackServer.Utility.Security
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; private set; }
    }
}
