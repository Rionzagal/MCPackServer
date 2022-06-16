using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Services.Interfaces;
using MCPackServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Security;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPackServer.Services
{
    public class BaseService : IBaseService
    {
        public MCPACKDBContext _context;
        public readonly IConfiguration _config;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseService(MCPACKDBContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor; 
            _context = context;
            _config = config;
        }

        public IDbConnection Connection
        {
            get { return new MySqlConnection(_config.GetConnectionString("MySqlConnection")); }
        }

        protected List<KeyValuePair<string, string>> CheckFilters(List<WhereFilter>? input)
        {
            List<KeyValuePair<string, string>> filters = new();
            if (null != input && input.Any())
            {
                foreach (var item in input)
                {
                    if (!string.IsNullOrEmpty(item.Value)) 
                        filters.Add(new KeyValuePair<string, string>(item.Field, item.Value));
                }
            }
            return filters;
        }

        protected async Task LogResponse<T>(ActionResponse<T> response)
        {
            try
            {
                string userName = System.Security.Claims.ClaimsPrincipal.Current?.Identity?.Name ?? string.Empty;
                string CurrentUserId = _context.AspNetUsers.Where(u => userName == u.UserName).FirstOrDefault()?.Id ?? 
                    _context.AspNetUsers.First().Id;
                string jsonMessage = response.IsSuccessful
                    ? Newtonsoft.Json.JsonConvert.SerializeObject(response.Value)
                    : Newtonsoft.Json.JsonConvert.SerializeObject(response.Errors);
                Entities.Logs newLog = new()
                {
                    UserId = CurrentUserId,
                    Message = response.IsSuccessful ? Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            Message = $"Action: {response.Action} completed successfuly in table {response.Value?.GetType().Name}",
                            response.Value
                        }) : Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            Message = $"Action: {response.Action} failed causing errors in table {response.Value?.GetType().Name}",
                            response.Value,
                            response.Errors
                        }),
                    Action = response.Action.ToString(),
                    Succeeded = response.IsSuccessful,
                    TableName = response.Value?.GetType().Name ?? "Not available",
                    Exception = response.IsSuccessful ? "N/A" : response.ExceptionText,
                    TimeOfAction = DateTime.Now
                };
                await _context.AddAsync(newLog);
                await _context.SaveChangesAsync();
                _context.Entry(newLog).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "", bool getAll = false) where T : class
        {
            using IDbConnection conn = Connection;
            var instance = Activator.CreateInstance(typeof(T));
            string tableName = instance.GetType().Name;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT * FROM {tableName} ";
            if (whereValues.Any())
            {
                string where = "WHERE ";
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                    if (item.Key != whereValues.Last().Key) where += "AND ";
                }
                query += where;
            }
            query += $"ORDER BY {sortField} {order} ";
            if (!getAll) 
                query += $"LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<T>(query, parameters);
        }

        public virtual async Task<int?> GetTotalCountAsync<T>(DataManagerRequest request, string countField = "Id") where T : class
        {
            using IDbConnection conn = Connection;
            var instance = Activator.CreateInstance(typeof(T));
            string tableName = instance.GetType().Name;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereValues = CheckFilters(request.Where);
            string query = $"SELECT COUNT({countField}) FROM {tableName} ";
            if (whereValues.Any())
            {
                string where = "WHERE ";
                foreach (var item in whereValues)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                    if (item.Key != whereValues.Last().Key) where += "AND ";
                }
                query += where;
            }
            return await conn.ExecuteScalarAsync<int?>(query, parameters);
        }

        public virtual async Task<T> GetByKeyAsync<T>(object value, string key = "Id") where T : class
        {
            using IDbConnection conn = Connection;
            var instance = Activator.CreateInstance(typeof(T));
            string tableName = instance.GetType().Name;
            DynamicParameters parameters = new();
            parameters.Add(key, value);
            string query = $"SELECT * FROM {tableName} WHERE {key} LIKE CONCAT('%', @{key}, '%') ";
            return await conn.QuerySingleAsync<T>(query, parameters);
        }

        public async Task<ActionResponse<T>> AddAsync<T>(T entity)
        {
            ActionResponse<T> response = new(entity, Actions.Insert);
            try
            {
                _ = entity ?? throw new ArgumentNullException(nameof(entity));
                var properties = entity.GetType().GetProperties()
                    .Where(x => !x.GetAccessors()[0].IsFinal && x.GetAccessors()[0].IsVirtual).ToList();
                properties.ForEach(x => x.SetValue(entity, null));
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
                response.AttachValue(entity);
                response.Success();
            }
            catch (Exception ex)
            {
                response.Failure(ex);
            }
            await LogResponse(response);
            return response;
        }

        public virtual async Task<ActionResponse<T>> UpdateAsync<T>(T entity)
        {
            ActionResponse<T> response = new(entity, Actions.Update);
            try
            {
                _ = entity ?? throw new ArgumentNullException(nameof(entity));
                var properties = entity.GetType().GetProperties()
                    .Where(x => !x.GetAccessors()[0].IsFinal && x.GetAccessors()[0].IsVirtual).ToList();
                properties.ForEach(x => x.SetValue(entity, null));
                _context.Update(entity);
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
                response.AttachValue(entity);
                response.Success();
            }
            catch (Exception ex)
            {
                response.Failure(ex);
            }
            await LogResponse(response);
            return response;
        }

        public virtual async Task<ActionResponse<T>> RemoveAsync<T>(T entity)
        {
            using IDbConnection conn = Connection;
            conn.Open();
            using IDbTransaction transaction = conn.BeginTransaction();
            ActionResponse<T> response = new(entity, Actions.Delete);
            try
            {
                _ = entity ?? throw new ArgumentNullException(nameof(entity));
                DynamicParameters parameters = new();
                string tableName = entity.GetType().Name;
                string query = $"DELETE FROM {tableName} WHERE ";
                var properties = entity.GetType()
                    .GetProperties()
                    .Where(x => x.GetAccessors()[0].IsFinal || !x.GetAccessors()[0].IsVirtual)
                    .ToList();
                if (properties.Any(x => "Id" == x.Name))
                {
                    var id = properties.Single(x => "Id" == x.Name);
                    query += $"Id = {id.GetValue(entity)}";
                }
                else
                {
                    foreach (var property in properties)
                    {
                        object? value = property.GetValue(entity);
                        if (null != value)
                        {
                            parameters.Add(property.Name, value);
                            if (property.Name != properties.First().Name) query += "AND ";
                            query += $"{property.Name} LIKE CONCAT('%', @{property.Name}, '%') ";
                        }
                    }
                }
                await conn.ExecuteAsync(query, parameters, transaction);
                transaction.Commit();
                response.AttachValue(entity);
                response.Success();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                conn.Close();
                response.Failure(ex);
            }
            await LogResponse(response);
            return response;
        }
    }
}
