using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPackServer.Services
{
    public class FamiliesService : BaseService, IFamiliesService
    {
        public FamiliesService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "")
        {
            using IDbConnection conn = Connection;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT f.*, g.* FROM ArticleFamilies f " +
                $"INNER JOIN ArticleGroups g ON f.GroupId = g.Id ";
            if (whereValues.Any())
            {
                string where = "WHERE ";
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"f.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                    if (whereValues.Last().Key != item.Key)
                        where += "AND ";
                }
                query += where;
            }
            query += $" ORDER BY f.{sortField} {order} LIMIT {request.Skip}, {request.Take}";
            return await conn.QueryAsync<ArticleFamilies, ArticleGroups, ArticleFamilies>
                (query, (families, group) =>
                {
                    families.Group = group;
                    return families;
                }, param: parameters) as IEnumerable<T>;
        }

        public override async Task<int?> GetTotalCountAsync<T>(DataManagerRequest request)
        {
            using IDbConnection conn = Connection;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT f.*, g.* FROM ArticleFamilies f " +
                $"INNER JOIN ArticleGroups g ON f.GroupId = g.Id ";
            if (whereValues.Any())
            {
                string where = "WHERE ";
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"f.{item.Key} LIKE '%' + @{item.Key} + '%' ";
                    if (item.Key != whereValues.Last().Key)
                        where += "AND ";
                }
                query += where;
            }
            return await conn.ExecuteScalarAsync<int?>(query, parameters);
        }
    }
}
