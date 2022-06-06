using Dapper;
using MCPackServer.Data.Entity;
using MCPackServer.Entities;
using MCPackServer.Models;
using MCPackServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPackServer.Services
{
    public class FamiliesService : BaseService, IFamiliesService
    {
        public FamiliesService(MCPACKDBContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(context, config, httpContextAccessor)
        {
        }

        
    }
}
