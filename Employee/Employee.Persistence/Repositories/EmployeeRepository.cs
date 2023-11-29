using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Persistence;

namespace Employee.Persistence.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee.Domain.Entities.Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeDbContext dbContext) : base(dbContext)
        {
        }
    }
}
