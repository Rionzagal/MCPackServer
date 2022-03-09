using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class ProjectsService : BaseService, IProjectsService
    {
        public ProjectsService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "")
        {
            using IDbConnection conn = Connection;
            request.Take = 0!= request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            string query = $"SELECT p.*, c.* FROM Projects p INNER JOIN Clients c " +
                $"ON p.ClientId = c.Id ";
            if (null != request.Where && request.Where.Any())
            {
                string where = "WHERE ";
                foreach (var item in request.Where)
                {
                    parameters.Add($"@{item.Field}", item.Value);
                    where += $"p.{item.Field} LIKE '%' + @{item.Field} + '%' ";
                    if (item.Field != request.Where.Last().Field) where += "AND ";
                }
                query += where;
            }
            query += $"ORDER BY {sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<Projects, Clients, Projects>
                (query, param: parameters, map: (project, client) =>
                {
                    project.Client = client;
                    return project;
                }) as IEnumerable<T>;
        }

        public override async Task<T> GetByKeyAsync<T>(object value, string key = "Id")
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            parameters.Add("@" + key, value);
            string query = $"SELECT p.*, c.* FROM Projects p INNER JOIN " +
                $"Clients c ON p.ClientId = c.Id WHERE p.Id = @key";
            var result = await conn.QueryAsync<Projects, Clients, Projects>(query, param: parameters, map: (project, client) =>
            {
                project.Client = client;
                return project;
            });
            return result.FirstOrDefault() as T;
        }
    }
}
