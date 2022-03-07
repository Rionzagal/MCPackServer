using Dapper;
using DataAccess.Models;
using DataAccess.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPackServer.EntityServices
{
    public class BaseService
    {
        private readonly IMySqlDataAccess data;
        private readonly IConfiguration config;

        public string ConnectionString { get; set; }

        public BaseService(IMySqlDataAccess _data, IConfiguration _config)
        {
            data = _data;
            config = _config;

            ConnectionString = config.GetConnectionString("MySqlConnection");
        }

        public async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "") where T : class
        {
            var instance = Activator.CreateInstance(typeof(T));
            string tableName = instance.GetType().Name;
            string query = $"SELECT * FROM {tableName} ";
            DynamicParameters parameters = new();
            if (request.Where.Any())
            {
                string where = "WHERE ";
                foreach (var item in request.Where)
                {
                    parameters.Add($"@{item.Key}", item.Value);
                    where += $"{item.Key} LIKE '%' + @{item.Key} + '%' ";
                    if (item.Key != request.Where.Last().Key) where += "AND ";
                }
                query += where;
            }
            query += $"ORDER BY {sortField} {order} OFFSET {request.Skip} ROWS FETCH NEXT {request.Take} ROWS ONLY";
            return await data.LoadData<T>(query, parameters, ConnectionString);
        }

        public async Task<int?> GetTotalCountAsync<T>(DataManagerRequest request) where T : class
        {
            var instance = Activator.CreateInstance(typeof(T));
            string tableName = instance.GetType().Name;
            string query = $"SELECT COUNT (Id) FROM {tableName} ";
            DynamicParameters parameters = new();
            if (request.Where.Any())
            {
                string where = "WHERE ";
                foreach (var item in request.Where)
                {
                    parameters.Add($"@{item.Key}", item.Value);
                    where += $"{item.Key} LIKE '%' + @{item.Key} + '%' ";
                    if (item.Key != request.Where.Last().Key) where += "AND ";
                }
                query += where;
            }
            return await data.GetCount(query, parameters, ConnectionString);
        }

        public async Task<T> GetByKeyAsync<T>(object value, string key = "Id") where T : class
        {
            var instance = Activator.CreateInstance(typeof(T));
            string tableName = instance.GetType().Name;
            DynamicParameters parameters = new();
            parameters.Add(key, value);
            string query = $"SELECT * FROM {tableName} WHERE {key} LIKE '%' + @{key} +'%' ";
            var entities = await data.LoadData<T>(query, parameters, ConnectionString);
            if (entities.Any())
                return entities.First();
            else
                return null;
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            string tableName = entity.GetType().Name;
            string propertyNames = string.Empty;
            string propertyValues = string.Empty;
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                propertyNames += property.Name;
                propertyValues += $"@{property.Name}";
                if (property.Name != properties.Last().Name)
                {
                    propertyNames += ", ";
                    propertyValues += ", ";
                }
            }
            string query = $"INSERT INTO {tableName} ({propertyNames}) VALUES({propertyValues})";
            await data.SaveData<T>(query, entity, ConnectionString);
        }

        public async Task RemoveAsync<T>(T entity) where T : class
        {
            string tableName = entity.GetType().Name;
            string query = $"DELETE FROM {tableName} WHERE ";
            var properties = entity.GetType().GetProperties();
            if (properties.Any(p => p.Name == "Id"))
            {

            }
        }
    }
}
