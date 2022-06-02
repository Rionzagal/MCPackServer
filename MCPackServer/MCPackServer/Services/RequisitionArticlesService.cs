using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class RequisitionArticlesService : BaseService, IRequisitionArticlesService
    {
        public RequisitionArticlesService(MCPACKDBContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(context, config, httpContextAccessor)
        {
        }

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "", bool getAll = false)
        {
            using IDbConnection conn = Connection;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            string query = $"SELECT ra.*, r.*, a.*, p.*, f.*, g.* FROM RequisitionArticles ra " +
                $"INNER JOIN Requisitions r ON ra.RequisitionId = r.Id " +
                $"INNER JOIN PurchaseArticles a ON ra.ArticleId = a.Id " +
                $"INNER JOIN Projects p ON ra.ProjectId = p.Id " +
                "INNER JOIN ArticleFamilies f ON a.FamilyId = f.Id " +
                "INNER JOIN ArticleGroups g ON f.GroupId = g.Id ";
            if (null != request.Where && request.Where.Any())
            {
                string where = "WHERE ";
                foreach (var item in request.Where)
                {
                    parameters.Add("@" + item.Field, item.Value);
                    where += $"ra.{item.Field} LIKE CONCAT('%', @{item.Field}, '%') ";
                    if (item.Field != request.Where.Last().Field) where += "AND ";
                }
                query += where;
            }
            query += $"ORDER BY ra.{sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync
                <RequisitionArticles, Requisitions, PurchaseArticles, Projects, ArticleFamilies, ArticleGroups, RequisitionArticles>
                (query, param: parameters, map: (requisitionArticle, requisition, article, project, family, group) =>
                {
                    requisitionArticle.Requisition = requisition;
                    requisitionArticle.Article = article;
                    requisitionArticle.Project = project;
                    requisitionArticle.Article.Family = family;
                    requisitionArticle.Article.Family.Group = group;
                    return requisitionArticle;
                }) as IEnumerable<T>;
        }

        public override async Task<int?> GetTotalCountAsync<T>(DataManagerRequest request)
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            string query = $"SELECT COUNT(ArticleId) FROM RequisitionArticles ";
            if (null != request.Where && request.Where.Any())
            {
                string where = "WHERE ";
                foreach (var item in request.Where)
                {
                    parameters.Add(item.Field, item.Value);
                    where += $"ra.{item.Field} LIKE CONCAT('%', @{item.Field}, '%') ";
                    if (item.Field != request.Where.Last().Field) where += "AND ";
                }
                query += where;
            }
            return await conn.ExecuteScalarAsync<int?>(query, parameters);
        }
    }
}
