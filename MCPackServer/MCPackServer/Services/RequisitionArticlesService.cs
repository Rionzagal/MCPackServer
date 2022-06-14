using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System.Data;

namespace MCPackServer.Services
{
    public class RequisitionArticlesService : BaseService, IRequisitionArticlesService
    {
        public RequisitionArticlesService(MCPACKDBContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(context, config, httpContextAccessor)
        {
        }

        
    }
}
