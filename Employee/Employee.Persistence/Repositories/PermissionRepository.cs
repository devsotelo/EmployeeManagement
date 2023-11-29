using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Persistence;
using Employee.Domain.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Employee.Persistence.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(EmployeeDbContext dbContext) : base(dbContext)
        {
        }

        public Task<bool> IsPermissionUnique(int idEmployee, int idPermissionType)
        {
            var matches = dbContext.Permissions.Any(e => e.EmployeeId.Equals(idEmployee) && e.PermissionTypeId.Equals(idPermissionType));
            return Task.FromResult(matches);
        }
    }
}
