using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class ProjectProductsService : BaseService, IProjectProductsService
    {
        public ProjectProductsService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "")
        {
            using IDbConnection conn = Connection;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            string query = $"SELECT pp.*, project.*, product.* FROM ProjectProducts pp " +
                $"INNER JOIN Projects project ON pp.ProjectId = project.Id " +
                $"INNER JOIN MCProducts product ON pp.ProductId = product.Id ";
            if (whereFilters.Any())
            {
                string where = "WHERE ";
                foreach (var item in whereFilters)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"pp.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                    if (whereFilters.Last().Key != item.Key)
                        where += "AND ";
                }
                query += where;
            }
            query += $" ORDER BY pp.{sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<ProjectProducts, Projects, MCProducts, ProjectProducts>
                    (query, (pp, project, product) =>
                    {
                        pp.Project = project;
                        pp.Product = product;
                        return pp;
                    }, param: parameters) as IEnumerable<T>;
        }

        public override async Task<int?> GetTotalCountAsync<T>(DataManagerRequest request)
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            string query = $"SELECT COUNT(ProductId) FROM ProjectProducts ";
            if (whereFilters.Any())
            {
                string where = "WHERE ";
                foreach (var item in whereFilters)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"pp.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                    if (whereFilters.Last().Key != item.Key)
                        where += "AND ";
                }
                query += where;
            }
            return await conn.ExecuteScalarAsync<int?>(query, parameters);
        }


    }
}
