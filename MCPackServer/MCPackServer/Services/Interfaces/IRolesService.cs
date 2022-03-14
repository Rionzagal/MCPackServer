using MCPackServer.Entities;

namespace MCPackServer.Services.Interfaces
{
    public interface IRolesService : IBaseService
    {
        Task<IEnumerable<AspNetUserRoles>> SearchUsersForRole(string roleId);
        Task<IEnumerable<AspNetUsers>> SearchUnassignedUsers(string roleId);
    }
}
