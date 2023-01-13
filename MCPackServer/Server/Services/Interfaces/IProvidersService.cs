using MCPackServer.Entities;
using MCPackServer.Models;

namespace MCPackServer.Services.Interfaces
{
    public interface IProvidersService : IBaseService
    {
        Task<ActionResponse<List<ProviderContacts>>> ClearContacts(object providerId);
        Task<int?> CountByQuote(object articleId, DataManagerRequest request);
        Task<int?> CountContacts(object providerId, DataManagerRequest request);
        Task<IEnumerable<Contacts>> GetAvailableContacts(object providerId, DataManagerRequest request, string sortField = "Id", string order = "");
        Task<IEnumerable<Providers>> GetByQuote(object articleId, DataManagerRequest request, string sortfield = "Id", string order = "");
        Task<IEnumerable<Contacts>> GetContacts(object providerId, DataManagerRequest request, string sortField = "Id", string order = "");
        Task<ActionResponse<ProviderContacts>> LinkContact(object providerId, object contactId);
        Task<ActionResponse<ProviderContacts>> RemoveContact(object providerId, object contactId);
    }
}