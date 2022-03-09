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

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "")
        {
            using IDbConnection conn = Connection;
            request.Take = 0!= request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            string query = $"SELECT q.*, a.*, p.* FROM Quotes q " +
                $"INNER JOIN PurchaseArticles a ON q.ArticleId = a.Id " +
                $"INNER JOIN Providers p ON q.ProviderId = p.Id ";
            if (null != request.Where && request.Where.Any())
            {
                string where = "WHERE ";
                foreach (var item in request.Where)
                {
                    parameters.Add("@" + item.Field, item.Value);
                    where += $"q.{item.Field} LIKE '%' + @{item.Field} + '%' ";
                    if (item.Field != request.Where.Last().Field) where += "AND ";
                }
                query += where;
            }
            query += $"ORDER BY {sortField} {order} LIMIT {request.Skip}, {request.Take} ";
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
