
using MCPackServer.Entities;
using MCPackServer.Models;

namespace MCPackServer.Services.Interfaces
{
    public interface IContactsService : IBaseService
    {
        Task<ActionResponse<List<Contacts>>> ClearUnaligned();
    }
}