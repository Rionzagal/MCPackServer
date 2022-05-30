using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class ContactsService : BaseService, IContactsService
    {
        public ContactsService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public async Task<ActionResponse<List<Contacts>>> ClearUnaligned()
        {
            using IDbConnection conn = Connection;
            conn.Open();
            using IDbTransaction transaction = conn.BeginTransaction();
            List<Contacts> contacts = _context.Contacts
                .Where(c => !_context.AssociatedContactsView
                    .Select(ac => ac.Id)
                    .Contains(c.Id))
                .ToList();
            ActionResponse<List<Contacts>> response = new(contacts, Actions.Delete);
            try
            {
                string query = "DELETE FROM Contacts WHERE Id NOT IN " +
                    $"(SELECT Id FROM AssociatedContactsView)";
                await conn.ExecuteAsync(query, transaction: transaction);
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
