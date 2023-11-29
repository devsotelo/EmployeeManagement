using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Domain.Entities;

namespace Employee.Application.Contracts.Persistence
{
    public interface IPermissionRepository : IAsyncRepository<Permission>
    {
        Task<bool> IsPermissionUnique(int idEmployee, int idPermissionType);
    }
}
