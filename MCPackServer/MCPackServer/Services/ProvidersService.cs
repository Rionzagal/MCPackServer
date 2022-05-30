using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MCPackServer.Services
{
    public class ProvidersService : BaseService, IProvidersService
    {
        public ProvidersService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public async Task<IEnumerable<Providers>> GetByQuote(object articleId, DataManagerRequest request, string sortfield = "Id", string order = "")
        {
            using IDbConnection conn = Connection;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            string query = $"SELECT p.* FROM Providers p INNER JOIN Quotes q ON p.Id = q.ProviderId " +
                    $"WHERE q.ArticleId = {articleId} ";
            if (whereFilters.Any())
            {
                string where = string.Empty;
                foreach (var item in whereFilters)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND p.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            query += $"ORDER BY p.{sortfield} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<Providers>(query, parameters);
        }

        public async Task<int?> CountByQuote(object articleId, DataManagerRequest request)
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            string query = $"SELECT COUNT(p.Id) FROM Providers p INNER JOIN Quotes q ON p.Id = q.ProviderId " +
                    $"WHERE q.ArticleId = {articleId} ";
            if (whereFilters.Any())
            {
                string where = string.Empty;
                foreach (var item in whereFilters)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND p.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            return await conn.ExecuteScalarAsync<int?>(query, parameters);
        }

        public async Task<IEnumerable<Contacts>> GetContacts(object providerId, DataManagerRequest request, string sortField = "Id", string order = "")
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            request.Take = 0 != request.Take ? request.Take : 10;
            string query = $"SELECT * FROM Contacts U INNER JOIN ProviderContacts C ON U.Id = C.ContactId WHERE ProviderId = {providerId} ";
            if (whereFilters.Any())
            {
                string where = string.Empty;
                foreach (var item in whereFilters)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND U.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            query += $"ORDER BY U.{sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<Contacts>(query, parameters);
        }

        public async Task<int?> CountContacts(object providerId, DataManagerRequest request)
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            request.Take = 0 != request.Take ? request.Take : 10;
            string query = $"SELECT COUNT(U.Id) FROM Contacts U INNER JOIN ProviderContacts C ON U.Id = C.ContactId WHERE ProviderId = {providerId} ";
            if (whereFilters.Any())
            {
                string where = string.Empty;
                foreach (var item in whereFilters)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND p.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            return await conn.ExecuteScalarAsync<int?>(query, parameters);
        }

        public async Task<IEnumerable<Contacts>> GetAvailableContacts(object providerId, DataManagerRequest request, string sortField = "Id", string order = "")
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            request.Take = 0 != request.Take ? request.Take : 10;
            string query = $"SELECT * FROM Contacts C WHERE C.Id NOT IN(SELECT ContactId FROM ProviderContacts WHERE ProviderId = {providerId})";
            if (whereFilters.Any())
            {
                string where = string.Empty;
                foreach (var item in whereFilters)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND p.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            query += $"ORDER BY C.{sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<Contacts>(query, parameters);
        }

        public async Task<ActionResponse<ProviderContacts>> LinkContact(object providerId, object contactId)
        {
            ActionResponse<ProviderContacts> response = new("Add");
            ProviderContacts Link = new() { ProviderId = (int)providerId, ContactId = (int)contactId };
            try
            {
                await _context.AddAsync(Link);
                await _context.SaveChangesAsync();
                _context.Entry(Link).State = EntityState.Detached;
                response.Success();
                response.AttachValue(Link);
            }
            catch (Exception ex)
            {
                response.Failure(ex);
            }
            await LogResponse(response, "");
            return response;
        }

        public async Task<ActionResponse<ProviderContacts>> RemoveContact(object providerId, object contactId)
        {
            using IDbConnection conn = Connection;
            conn.Open();
            using IDbTransaction transaction = conn.BeginTransaction();
            ActionResponse<ProviderContacts> response = new("Delete");
            DynamicParameters parameters = new();
            try
            {
                parameters.Add(nameof(providerId), providerId);
                parameters.Add(nameof(contactId), contactId);
                string query = $"DELETE FROM ProviderContacts WHERE ProviderId = @providerId AND ContactId = @contactId";
                await conn.ExecuteAsync(query, parameters, transaction);
                transaction.Commit();
                response.Success();
            }
            catch (Exception ex)
            {
                conn.Close();
                transaction.Rollback();
                response.Failure(ex);
            }
            await LogResponse(response, "");
            return response;
        }

        public async Task<ActionResponse<ProviderContacts>> ClearContacts(object providerId)
        {
            using IDbConnection conn = Connection;
            conn.Open();
            using IDbTransaction transaction = conn.BeginTransaction();
            ActionResponse<ProviderContacts> response = new("Delete");
            DynamicParameters parameters = new();
            parameters.Add(nameof(providerId), providerId);
            string query = "DELETE FROM ProviderContacts WHERE ProviderId = @providerId";
            try
            {
                await conn.ExecuteAsync(query, parameters, transaction);
                transaction.Commit();
                response.Success();
            }
            catch (Exception ex)
            {
                conn.Close();
                transaction.Rollback();
                response.Failure(ex);
            }
            await LogResponse(response, "");
            return response;
        }
    }
}
