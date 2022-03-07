
using Dapper;

namespace DataAccess.Services.Interfaces
{
    public interface IMySqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T>(string sql, DynamicParameters parameters = null, string connectionString = null);
        Task<int?> GetCount(string sql, DynamicParameters parameters = null, string connectionString = null);
        Task SaveData<T>(string sql, T parameters, string connectionString = null);
    }
}