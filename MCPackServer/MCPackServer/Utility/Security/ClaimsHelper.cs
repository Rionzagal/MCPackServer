using MCPackServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace MCPackServer.Utility.Security
{
    public static class ClaimsHelper
    {
        public static void GetPermissions(this List<RoleClaimsViewModel> allPermissions, Type policy, string roleId)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                allPermissions.Add(new RoleClaimsViewModel { Value = field.GetValue(null).ToString(), Type = "Permissions" });
            }
        }

        public static async Task AddPermissionClaim(RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", permission));
            }
        }
    }
}
