using MCPackServer.Entities;
using MCPackServer.Models;

namespace MCPackServer.Services.Interfaces
{
    public interface IClientsService : IBaseService
    {
        Task<ActionResponse<ClientContacts>> ClearContacts(object clientId);
        Task<int?> CountContacts(string clientId, DataManagerRequest request);
        Task<IEnumerable<Contacts>> GetAvailableContacts(string clientId, DataManagerRequest request, string sortField = "Id", string order = "");
        Task<IEnumerable<Contacts>> GetContacts(string clientId, DataManagerRequest request, string sortField = "Id", string order = "");
        Task<ActionResponse<ClientContacts>> LinkContact(object clientId, object contactId);
        Task<ActionResponse<ClientContacts>> RemoveContact(object clientId, object contactId);
    }
}