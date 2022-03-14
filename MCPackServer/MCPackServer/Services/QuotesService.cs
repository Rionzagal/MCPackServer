using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class QuotesService : BaseService, IQuotesService
    {
        public QuotesService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "", bool getAll = false)
        {
            using IDbConnection conn = Connection;
            request.Take = 0!= request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            string query = $"SELECT q.*, a.*, p.* FROM Quotes q " +
                $"INNER JOIN PurchaseArticles a ON q.ArticleId = a.Id " +
                $"INNER JOIN Providers p ON q.ProviderId = p.Id ";
            if (whereFilters.Any())
            {
                string where = string.Empty;
                foreach (var item in whereFilters)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND q.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            query += $"ORDER BY q.{sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<Quotes, PurchaseArticles, Providers, Quotes>
                (query, param: parameters, map: (quote, article, provider) =>
                {
                    quote.Article = article;
                    quote.Provider = provider;
                    return quote;
                }) as IEnumerable<T>;
        }
    }
}
