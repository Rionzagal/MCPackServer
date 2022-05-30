using MCPackServer.Entities;
using MCPackServer.Models;

namespace MCPackServer.Services.Interfaces
{
    public interface IClientsService : IBaseService
    {
        Task<ActionResponse<List<ClientContacts>>> ClearContacts(object clientId);
        Task<int?> CountContacts(object clientId, DataManagerRequest request);
        Task<IEnumerable<Contacts>> GetAvailableContacts(object clientId, DataManagerRequest request, string sortField = "Id", string order = "");
        Task<IEnumerable<Contacts>> GetContacts(object clientId, DataManagerRequest request, string sortField = "Id", string order = "");
        Task<ActionResponse<ClientContacts>> LinkContact(object clientId, object contactId);
        Task<ActionResponse<ClientContacts>> RemoveContact(object clientId, object contactId);
    }
}