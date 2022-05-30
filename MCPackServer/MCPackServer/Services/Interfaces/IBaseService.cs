using MCPackServer.Models;
using System.Data;

namespace MCPackServer.Services.Interfaces
{
    public interface IBaseService
    {
        IDbConnection Connection { get; }

        Task<ActionResponse<T>> AddAsync<T>(T entity, string userId = "");
        Task<T> GetByKeyAsync<T>(object value, string key = "Id") where T : class;
        Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "", bool getAll = false) where T : class;
        Task<int?> GetTotalCountAsync<T>(DataManagerRequest request) where T : class;
        Task<ActionResponse<T>> RemoveAsync<T>(T entity, string userId = "");
        Task<ActionResponse<T>> UpdateAsync<T>(T entity, string userId = "");
    }
}