using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class PurchaseOrdersService : BaseService, IPurchaseOrdersService
    {
        public PurchaseOrdersService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "", bool getAll = false)
        {
            using IDbConnection conn = Connection;
            request.Take = 0!= request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            List<KeyValuePair<string, string>> whereFilters = CheckFilters(request.Where);
            string query = $"SELECT PO.*, projects.*, providers.*, requisition.* FROM PurchaseOrders PO " +
                $"INNER JOIN Projects projects ON PO.ProjectId = projects.Id " +
                $"INNER JOIN Providers providers ON PO.ProviderId = providers.Id " +
                $"INNER JOIN Requisitions requisition ON PO.RequisitionId = requisition.Id ";
            if (whereFilters.Any())
            {
                string where = string.Empty;
                foreach (var item in whereFilters)
                {
                    parameters.Add(item.Key, item.Value);
                    where += $"AND PO.{item.Key} LIKE CONCAT('%', @{item.Key}, '%') ";
                }
                query += where;
            }
            query += $"ORDER BY PO.{sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<PurchaseOrders, Projects, Providers, Requisitions, PurchaseOrders>
                (query, param: parameters, map: (order, project, provider, requisition) =>
                {
                    order.Project = project;
                    order.Provider = provider;
                    order.Requisition = requisition;
                    return order;
                }) as IEnumerable<T>;
        }

        public override async Task<T> GetByKeyAsync<T>(object value, string key = "Id")
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            parameters.Add(key, value);
            string query = $"SELECT PO.*, projects.*, providers.*, requisition.* FROM PurchaseOrders PO " +
                $"INNER JOIN Projects projects ON PO.ProjectId = projects.Id " +
                $"INNER JOIN Providers providers ON PO.ProviderId = providers.Id " +
                $"INNER JOIN Requisitions requisition ON PO.RequisitionId = requisition.Id " +
                $"WHERE PO.{key} = @{key} ";
            var entities = await conn.QueryAsync<PurchaseOrders, Projects, Providers, Requisitions, PurchaseOrders>
                (query, (order, project, provider, requisition) =>
                {
                    order.Provider = provider;
                    order.Project = project;
                    order.Requisition = requisition;
                    return order;
                });
            return entities.FirstOrDefault() as T;
        }
    }
}
