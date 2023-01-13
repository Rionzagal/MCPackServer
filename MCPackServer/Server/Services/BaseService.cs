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
using System.Reflection;

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

        /// <summary>
        /// It takes a list of WhereFilter objects and returns a string that can be used in a SQL query
        /// </summary>
        /// <param name="input">List of WhereFilter objects</param>
        /// <param
        /// name="DynamicParameters">https://github.com/StackExchange/Dapper/blob/master/Dapper/DynamicParameters.cs</param>
        /// <returns>
        /// A string that is the where clause of a SQL query.
        /// </returns>
        protected string AddWhereClauseToQuery(List<WhereFilter>? input, DynamicParameters parameters)
        {
            string clause = string.Empty;
            if (null != input && input.Any(x =>
                {
                    if (null != x.Value && typeof(string) == x.Value.GetType() && string.IsNullOrEmpty((string)x.Value))
                        x.Value = null;
                    return null != x.Value;
                }))
            {
                clause = "WHERE ";
                List<WhereFilter> validFilters = input.Where(x =>
                    {
                        bool result = x.Value != null;
                        if (x.Value != null && typeof(string) == x.Value.GetType())
                            result = !string.IsNullOrEmpty((string)x.Value);
                        return result;
                    }).AsList();
                foreach (var item in validFilters)
                {
                    if (null != item.Value)
                    {
                        if (typeof(bool) == item.Value.GetType()) // When the filter is of type bool
                        {
                            parameters.Add(item.Field, (bool)item.Value ? "1" : "0");
                            clause += $"{item.Field} = @{item.Field} ";
                        }
                        else if (Operators.Between == item.Operator) // When the operator is set to Between
                        {
                            if (item.MinValue != null && item.MaxValue != null)
                            {
                                parameters.Add($"{item.Field}Min", item.MinValue);
                                parameters.Add($"{item.Field}Max", item.MaxValue);
                                clause += $"{item.Field} BETWEEN @{item.Field}Min AND @{item.Field}Max ";
                            }
                            else
                                throw new Exception("MinValue and MaxValue properties must be not null if the Operator is BETWEEN in WhereFilter object.");
                        }
                        else // When the operator is set to anything else
                        {
                            parameters.Add(item.Field, item.Value);
                            clause += $"{item.Field} {GetStartOperator(item.Operator)} @{item.Field} {GetEndOperator(item.Operator)} ";
                        }
                        if (validFilters.Last() != item)
                            clause += $"{input[input.FindIndex(x => x == item) + 1].Condition.ToString().ToUpper()} ";
                    }
                }
            }
            return clause;
        }

        /// <summary>
        /// It takes a list of `WhereFilter` objects, and returns a list of `KeyValuePair` objects
        /// </summary>
        /// <param name="input">List of WhereFilter objects</param>
        /// <returns>
        /// A list of KeyValuePair<string, string>
        /// </returns>
        protected List<KeyValuePair<string, string>> CheckFilters(List<WhereFilter>? input)
        {
            List<KeyValuePair<string, string>> filters = new();
            if (null != input && input.Any())
            {
                foreach (var item in input)
                {
                    if (null != item.Value)
                    {
                        if (item.Value is bool)
                            filters.Add(new KeyValuePair<string, string>(item.Field, ((bool)item.Value ? "1" : "0")));
                        else if (item.Value is string && !string.IsNullOrEmpty((string)item.Value))
                            filters.Add(new KeyValuePair<string, string>(item.Field, (string)item.Value));
                        else
                        {
                            string value = item.Value.ToString() ?? string.Empty;
                            if (!string.IsNullOrEmpty(value))
                                filters.Add(new KeyValuePair<string, string>(item.Field, value));
                        }
                    }
                }
            }
            return filters;
        }

        /// <summary>
        /// It logs the response of an action to the database
        /// </summary>
        /// <param name="response">The response object that is returned from the controller.</param>
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

        /// <summary>
        /// It takes a DataManagerRequest object, which contains a Where clause, a Skip value, a Take
        /// value, and a Select clause, and returns a list of objects of type T
        /// </summary>
        /// <param name="DataManagerRequest">This is a class that contains the following
        /// properties:</param>
        /// <param name="sortField">The field to sort by</param>
        /// <param name="order">"ASC" or "DESC"</param>
        /// <param name="getAll">if true, it will return all records, otherwise it will return the
        /// records based on the skip and take parameters.</param>
        /// <returns>
        /// A list of objects of type T.
        /// </returns>
        public virtual async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "", bool getAll = false) where T : class
        {
            using IDbConnection conn = Connection;
            string tableName = typeof(T).Name;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            string columns = GetColumnNamesForSelect<T>(request.Select);
            string query = $"SELECT {columns} FROM {tableName} ";
            query += AddWhereClauseToQuery(request.Where, parameters);
            query += $"ORDER BY {sortField} {order} ";
            if (!getAll)
                query += $"LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<T>(query, parameters);
        }

        /// <summary>
        /// It takes a DataManagerRequest object, which contains a Where clause, and returns the total
        /// number of records that match the Where clause
        /// </summary>
        /// <param name="DataManagerRequest">This is the request object that is sent from the client. It
        /// contains the filter, sort, and paging information.</param>
        /// <param name="countField">The field to count.</param>
        /// <returns>
        /// The total number of records in the table.
        /// </returns>
        public virtual async Task<int?> GetTotalCountAsync<T>(DataManagerRequest request, string countField = "Id") where T : class
        {
            using IDbConnection conn = Connection;
            string tableName = typeof(T).Name;
            DynamicParameters parameters = new();
            string query = $"SELECT COUNT({countField}) FROM {tableName} ";
            query += AddWhereClauseToQuery(request.Where, parameters);
            return await conn.ExecuteScalarAsync<int?>(query, parameters);
        }

        /// <summary>
        /// This function takes in a generic type, a value, and a key, and returns a single instance of
        /// the generic type from the database
        /// </summary>
        /// <param name="value">The value of the key you're searching for.</param>
        /// <param name="key">The name of the column to search by.</param>
        /// <returns>
        /// The method returns a single instance of the type T.
        /// </returns>
        public virtual async Task<T> GetByKeyAsync<T>(object value, string key = "Id") where T : class
        {
            using IDbConnection conn = Connection;
            var instance = Activator.CreateInstance(typeof(T));
            if (null == instance)
                throw new NullReferenceException("No valid type 'T' was given to the method when called.");
            string tableName = instance.GetType().Name;
            DynamicParameters parameters = new();
            parameters.Add(key, value);
            string query = $"SELECT * FROM {tableName} WHERE {key} = @{key} ";
            return await conn.QuerySingleAsync<T>(query, parameters);
        }

        /// <summary>
        /// It adds an entity to the database and returns an ActionResponse object with the entity and
        /// the action that was performed
        /// </summary>
        /// <param name="T">The entity type</param>
        /// <returns>
        /// An ActionResponse<T> object.
        /// </returns>
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

        /// <summary>
        /// This function updates an entity in the database
        /// </summary>
        /// <param name="T">The entity type</param>
        /// <returns>
        /// An ActionResponse<T> object.
        /// </returns>
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

        /// <summary>
        /// This function removes an entity from the database
        /// </summary>
        /// <param name="T">The type of entity to be removed.</param>
        /// <returns>
        /// An ActionResponse<T> object.
        /// </returns>
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

        /// <summary>
        /// It returns a string based on the input operator
        /// </summary>
        /// <param name="Operators">Contains, StartsWith, EndsWith, Equal, NotEqual, GreaterThan,
        /// IGreaterThan, LesserThan, ILesserThan</param>
        /// <returns>
        /// The operator that will be used in the query.
        /// </returns>
        protected string GetStartOperator(Operators input)
        {
            return input switch
            {
                (Operators.Contains) => " LIKE CONCAT('%', ",
                (Operators.StartsWith) => " LIKE CONCAT(",
                (Operators.EndsWith) => " LIKE CONCAT('%', ",
                (Operators.Equal) => " = ",
                (Operators.NotEqual) => "",
                (Operators.GreaterThan) => " > ",
                (Operators.IGreaterThan) => " >= ",
                (Operators.LesserThan) => " < ",
                (Operators.ILesserThan) => " <= ",
                _ => " LIKE CONCAT('%', "
            };
        }

        /// <summary>
        /// It returns the end operator for the input operator
        /// </summary>
        /// <param name="Operators">enum</param>
        /// <returns>
        /// The string value of the operator.
        /// </returns>
        protected string GetEndOperator(Operators input)
        {
            return input switch
            {
                (Operators.Contains) => ", '%') ",
                (Operators.StartsWith) => ", '%') ",
                (Operators.EndsWith) => ") ",
                (Operators.Equal) => " ",
                (Operators.NotEqual) => " ",
                (Operators.GreaterThan) => " ",
                (Operators.IGreaterThan) => " ",
                (Operators.LesserThan) => " ",
                (Operators.ILesserThan) => " ",
                _ => ", '%') "
            };
        }

        /// <summary>
        /// It takes a generic type and an optional list of strings and returns a string of comma
        /// separated column names
        /// </summary>
        /// <param name="select">A list of strings that represent the properties of the class you want
        /// to select.</param>
        /// <returns>
        /// The column names for the select statement.
        /// </returns>
        protected static string GetColumnNamesForSelect<T>(IEnumerable<string>? select = null)
        {
            string columnNames = string.Empty;
            var entityTypeProperties = typeof(T).GetProperties()
                .Where(x => x.GetAccessors()[0].IsPublic && (x.GetAccessors()[0].IsFinal || !x.GetAccessors()[0].IsVirtual));
            if (null == select || !select.Any())
            {
                foreach (var property in entityTypeProperties)
                {
                    columnNames += property.Name;
                    if (entityTypeProperties.Last() != property)
                        columnNames += ", ";
                }
            }
            else
            {
                foreach (var propertyName in select)
                {
                    if (!entityTypeProperties.Any(x => x.Name.ToLower() == propertyName.ToLower()))
                        throw new ArgumentException($"No property named {propertyName} in class {typeof(T).Name}");
                    columnNames += entityTypeProperties.Single(x => x.Name.ToLower() == propertyName.ToLower()).Name;
                    if (select.Last() != propertyName)
                        columnNames += ", ";
                }
            }
            return columnNames;
        }
    }
}
