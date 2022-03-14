using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class ArticlesToPurchaseService : BaseService, IArticlesToPurchaseService
    {
        public ArticlesToPurchaseService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "", bool getAll = false)
        {
            using IDbConnection conn = Connection;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT a.*, q.*, p.*, ar.* FROM ArticlesToPurchase a " +
                $"INNER JOIN Quotes q ON a.QuoteId = q.Id " +
                $"INNER JOIN PurchaseOrders p ON a.PurchaseOrderId = p.Id " +
                $"INNER JOIN PurchaseArticles ar ON q.ArticleId = ar.Id ";
            if (whereValues.Any())
            {
                string where = "WHERE ";
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"a.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                    if (item.Key != whereValues.Last().Key)
                        where += "AND ";
                }
                query += where;
            }
            query += $" ORDER BY a.{sortField} {order} LIMIT {request.Skip}, {request.Take}";
            return await conn.QueryAsync<ArticlesToPurchase, Quotes, PurchaseOrders, PurchaseArticles, ArticlesToPurchase>
                (query, (OrderedArticle, quote, order, article) =>
                {
                    OrderedArticle.Quote = quote;
                    OrderedArticle.PurchaseOrder = order;
                    OrderedArticle.Quote.Article = article;
                    return OrderedArticle;
                }, param: parameters) as IEnumerable<T>;
        }

        public override async Task<int?> GetTotalCountAsync<T>(DataManagerRequest request)
        {
            using IDbConnection conn = Connection;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT COUNT(a.QuoteId) FROM ArticlesToPurchase a " +
                $"INNER JOIN Quotes q ON a.QuoteId = q.Id " +
                $"INNER JOIN PurchaseOrders p ON a.PurchaseOrderId = p.Id " +
                $"INNER JOIN PurchaseArticles ar ON q.ArticleId = ar.Id ";
            if (whereValues.Any())
            {
                string where = "WHERE ";
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"a.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                    if (item.Key != whereValues.Last().Key)
                        where += "AND ";
                }
                query += where;
            }
            return await conn.ExecuteScalarAsync<int?>(query, parameters);
        }
    }
}
