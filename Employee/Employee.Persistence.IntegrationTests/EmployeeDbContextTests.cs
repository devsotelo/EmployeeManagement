using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Employee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Employee.Persistence.IntegrationTests
{
    public class EmployeeDbContextTests
    {
        private readonly EmployeeDbContext dbContext;

        public EmployeeDbContextTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EmployeeDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            dbContext = new EmployeeDbContext(dbContextOptions);
        }

        [Fact]
        public async void Save_PermissionType()
        {
            var permissionType = new PermissionType() { Id = 1, Name = "Test Permission" };

            dbContext.PermissionTypes.Add(permissionType);
            await dbContext.SaveChangesAsync();

            dbContext.PermissionTypes.ToList().Count.ShouldBe(1);
        }
    }
}