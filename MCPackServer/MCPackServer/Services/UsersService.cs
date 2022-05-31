using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class UsersService : BaseService, IUsersService
    {
        public UsersService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "", bool getAll = false)
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            request.Take = 0 != request.Take ? request.Take : 10;
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT a.*, i.* FROM AspNetUsers a INNER JOIN UserInformation i ON a.Id = i.AspNetUserId ";
            if (whereValues.Any())
            {
                string where = string.Empty;
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND a.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            query += $"ORDER BY a.{sortField} {order} ";
            if (!getAll) query += $"LIMIT {request.Skip} {request.Take} ";
            return await conn.QueryAsync<AspNetUsers, UserInformation, AspNetUsers>(
                query, param: parameters, map: (user, userInfo) =>
                {
                    user.UserInformation = userInfo;
                    return user;
                }) as IEnumerable<T>;
        }

        public override async Task<T> GetByKeyAsync<T>(object value, string key = "Id")
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            parameters.Add(key, value);
            string query = $"SELECT a.*, i.* FROM AspNetUsers a INNER JOIN UserInformation i ON a.Id = i.AspNetUserId " +
                $"WHERE a.{key} LIKE CONCAT('%', @{key}, '%') ";
            try
            {
                var result = await conn.QueryAsync<AspNetUsers, UserInformation, AspNetUsers>(
                query, param: parameters, map: (user, userInfo) =>
                {
                    user.UserInformation = userInfo;
                    return user;
                }, splitOn: "Id, AspNetUserId");
                return result.First() as T;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
