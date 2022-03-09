using MCPackServer.Entities;
using MCPackServer.Models;

namespace MCPackServer.Services.Interfaces
{
    public interface IProvidersService : IBaseService
    {
        Task ClearContacts(object providerId);
        Task<int?> CountByQuote(object articleId, DataManagerRequest request);
        Task<int?> CountContacts(string providerId, DataManagerRequest request);
        Task<IEnumerable<Contacts>> GetAvailableContacts(string providerId, DataManagerRequest request, string sortField = "Id", string order = "");
        Task<IEnumerable<Providers>> GetByQuote(object articleId, DataManagerRequest request, string sortfield = "Id", string order = "");
        Task<IEnumerable<Contacts>> GetContacts(string providerId, DataManagerRequest request, string sortField = "Id", string order = "");
        Task LinkContact(object providerId, object contactId);
        Task RemoveContact(object providerId, object contactId);
    }
}