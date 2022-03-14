using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using System.Data;
using MCPackServer.Services.Interfaces;

namespace MCPackServer.Services
{
    public class RolesService : BaseService, IRolesService
    {
        public RolesService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public async Task<IEnumerable<AspNetUserRoles>> SearchUsersForRole(string roleId)
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            parameters.Add(nameof(roleId), roleId);
            string query = $"SELECT ur.*, u.* FROM AspNetUserRoles ur INNER JOIN AspNetUsers u ON ur.UserId = u.Id " +
                $"WHERE ur.RoleId = @roleId";
            return await conn.QueryAsync<AspNetUserRoles, AspNetUsers, AspNetUserRoles>
                (query, param: parameters, map: (UserRole, User) =>
                {
                    UserRole.User = User;
                    return UserRole;
                }, splitOn: "UserId, RoleId");
        }

        public async Task<IEnumerable<AspNetUsers>> SearchUnassignedUsers(string roleId)
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            parameters.Add(nameof(roleId), roleId);
            string query = $"SELECT u.* FROM AspNetUsers u " +
                $"LEFT JOIN AspNetUserRoles ur ON ur.UserId = u.Id WHERE u.Id NOT IN " +
                $"(SELECT UserId FROM AspNetUserRoles WHERE RoleId = @roleId)";
            return await conn.QueryAsync<AspNetUsers>(query, param: parameters);
        }
    }
}
