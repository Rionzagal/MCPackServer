using Dapper;
using DataAccess.Services.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class MySqlDataAccess : IMySqlDataAccess
    {
        public async Task<IEnumerable<T>> LoadData<T>(string sql, DynamicParameters parameters = null, string connectionString = null)
        {
            using IDbConnection connection = new MySqlConnection(connectionString);
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<int?> GetCount(string sql, DynamicParameters parameters = null, string connectionString = null)
        {
            using IDbConnection connection = new MySqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int?>(sql, parameters);
        }

        public async Task SaveData<T>(string sql, T parameters, string connectionString = null)
        {
            using IDbConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            using IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                await connection.ExecuteAsync(sql, parameters, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                connection.Close();
                transaction.Rollback();
            }
        }
    }
}
