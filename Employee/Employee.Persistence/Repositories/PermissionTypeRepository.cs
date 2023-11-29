using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Persistence;
using Employee.Domain.Entities;

namespace Employee.Persistence.Repositories
{
    public class PermissionTypeRepository : BaseRepository<PermissionType>, IPermissionTypeRepository
    {
        public PermissionTypeRepository(EmployeeDbContext dbContext) : base(dbContext)
        {
        }
    }
}
