using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MCPackServer.Services
{
    public class ClientsService : BaseService, IClientsService
    {
        public ClientsService(MCPACKDBContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(context, config, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<Contacts>> GetContacts(object clientId, DataManagerRequest request, string sortField = "Id", string order = "")
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            request.Take = 0 != request.Take ? request.Take : 10;
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT * FROM Contacts U INNER JOIN ClientContacts C ON U.Id = C.ContactId WHERE ClientId = {clientId} ";
            if (whereValues.Any())
            {
                string where = string.Empty;
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND U.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            query += $"ORDER BY U.{sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<Contacts>(query, parameters);
        }

        public async Task<int?> CountContacts(object clientId, DataManagerRequest request)
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            request.Take = 0 != request.Take ? request.Take : 10;
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT COUNT(U.Id) FROM Contacts U INNER JOIN ClientContacts C ON U.Id = C.ContactId WHERE ClientId = {clientId} ";
            if (whereValues.Any())
            {
                string where = string.Empty;
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND U.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            return await conn.ExecuteScalarAsync<int?>(query, parameters);
        }

        public async Task<IEnumerable<Contacts>> GetAvailableContacts(object clientId, DataManagerRequest request, string sortField = "Id", string order = "")
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            request.Take = 0 != request.Take ? request.Take : 10;
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT * FROM Contacts C WHERE C.Id NOT IN(SELECT ContactId FROM ClientContacts WHERE ClientId = {clientId})";
            if (whereValues.Any())
            {
                string where = string.Empty;
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND C.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            query += $"ORDER BY C.{sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<Contacts>(query, parameters);
        }

        public async Task<ActionResponse<ClientContacts>> LinkContact(object clientId, object contactId)
        {
            ClientContacts Link = new() { ClientId = (int)clientId, ContactId = (int)contactId };
            ActionResponse<ClientContacts> response = new(Link, Actions.Insert);
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
            await LogResponse(response);
            return response;
        }
        public async Task<ActionResponse<ClientContacts>> RemoveContact(object clientId, object contactId)
        {
            using IDbConnection conn = Connection;
            conn.Open();
            using IDbTransaction transaction = conn.BeginTransaction();
            ActionResponse<ClientContacts> response = new(
                new ClientContacts() 
                { 
                    ClientId = (int)clientId,
                    ContactId = (int)contactId
                },
                Actions.Delete);
            DynamicParameters parameters = new();
            try
            {
                parameters.Add(nameof(clientId), clientId);
                parameters.Add(nameof(contactId), contactId);
                string query = $"DELETE FROM ClientContacts WHERE ClientId = @clientId AND ContactId = @contactId";
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
            await LogResponse(response);
            return response;
        }

        public async Task<ActionResponse<List<ClientContacts>>> ClearContacts(object clientId)
        {
            using IDbConnection conn = Connection;
            conn.Open();
            using IDbTransaction transaction = conn.BeginTransaction();
            List<ClientContacts> contacts = (
                from contact in _context.ClientContacts where contact.ClientId == (int)clientId select contact
                ).ToList();
            ActionResponse<List<ClientContacts>> response = new(contacts, Actions.Delete);
            DynamicParameters parameters = new();
            parameters.Add(nameof(clientId), clientId);
            string query = "DELETE FROM ClientContacts WHERE ClientId = @clientId";
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
            await LogResponse(response);
            return response;
        }
    }
}
