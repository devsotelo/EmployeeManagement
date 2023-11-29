using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Models;

namespace Employee.Application.Contracts.Infrastructure
{
    public interface IElasticService
    {
        Task<bool> Send(PermissionDocument permissionDocument);
    }
}
