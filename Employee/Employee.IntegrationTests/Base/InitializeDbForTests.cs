using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Domain.Entities;
using Employee.Persistence;
using EmptyFiles;
using Microsoft.EntityFrameworkCore;

namespace Employee.IntegrationTests.Base
{
    public static class InitializeDbForTestsExtensions
    {
        public static void InitializeDbForTests(this EmployeeDbContext context)
        {
            context.PermissionTypes.Add(new PermissionType
            {
                Id = 1,
                Name = "READ"
            });
            context.PermissionTypes.Add(new PermissionType
            {
                Id = 2,
                Name = "WRITE"
            });
            context.PermissionTypes.Add(new PermissionType
            {
                Id = 3,
                Name = "EXECUTE"
            });

            context.Employees.Add(new Employee.Domain.Entities.Employee
            {
                Id = 1,
                Name = "Francisco"
            });

            context.Employees.Add(new Employee.Domain.Entities.Employee
            {
                Id = 2,
                Name = "Arturo"
            });

            context.Permissions.Add(new Permission
            {
                Id = 1,
                EmployeeId = 1,
                PermissionTypeId = 1
            });

            context.SaveChanges();
        }
    }
}
