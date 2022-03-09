﻿using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class RequisitionsService : BaseService, IRequisitionsService
    {
        public RequisitionsService(MCPACKDBContext context, IConfiguration config) : base(context, config)
        {
        }

        public override async Task<IEnumerable<T>> GetForGridAsync<T>(DataManagerRequest request, string sortField = "Id", string order = "")
        {
            IDbConnection conn = Connection;
            request.Take = 0 != request.Take ? request.Take : 10;
            DynamicParameters parameters = new();
            string query = $"SELECT r.*, u.* FROM Requisitions r " +
               $"INNER JOIN AspNetUsers u ON r.UserId = u.Id ";
            if (null != request.Where && request.Where.Any())
            {
                string where = "WHERE ";
                foreach (var item in request.Where)
                {
                    parameters.Add("@" + item.Field, item.Value);
                    where += $"r.{item.Field} LIKE '%' + @{item.Field} + '%' ";
                    if (item.Field != request.Where.Last().Field) where += "AND ";
                }
                query += where;
            }
            query += $"ORDER BY {sortField} {order} LIMIT {request.Skip}, {request.Take} ";
            return await conn.QueryAsync<Requisitions, AspNetUsers, Requisitions>
                (query, param: parameters, map: (requisition, user) =>
                {
                    requisition.User = user;
                    return requisition;
                }) as IEnumerable<T>;
        }

        public override async Task<T> GetByKeyAsync<T>(object value, string key = "Id")
        {
            using IDbConnection conn = Connection;
            DynamicParameters parameters = new();
            parameters.Add("@" + key, value);
            string query = $"SELECT r.*, u.* FROM Requisitions r " +
                $"INNER JOIN AspNetUsers u ON r.UserId = u.Id WHERE r.{key} = @{key}";
            var entities = await conn.QueryAsync<Requisitions, AspNetUsers, Requisitions>
                (query, param: parameters, map: (requisition, user) =>
                {
                    requisition.User = user;
                    return requisition;
                });
            return entities.FirstOrDefault() as T;
        }
    }
}
