using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Domain.Entities;

namespace Employee.Application.Contracts.Persistence
{
    public interface IEmployeeRepository : IAsyncRepository<Employee.Domain.Entities.Employee>
    {
    }
}
