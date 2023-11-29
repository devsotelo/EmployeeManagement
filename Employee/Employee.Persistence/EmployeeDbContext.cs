using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Employee.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Employee.Persistence
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<Employee.Domain.Entities.Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeDbContext).Assembly);

            modelBuilder.Entity<PermissionType>().HasData(new PermissionType
            {
                Id = 1,
                Name = "READ"
            });
            modelBuilder.Entity<PermissionType>().HasData(new PermissionType
            {
                Id = 2,
                Name = "WRITE"
            });
            modelBuilder.Entity<PermissionType>().HasData(new PermissionType
            {
                Id = 3,
                Name = "EXECUTE"
            });

            modelBuilder.Entity<Employee.Domain.Entities.Employee>().HasData(new Employee.Domain.Entities.Employee
            {
                Id = 1,
                Name = "Francisco"
            });

            modelBuilder.Entity<Employee.Domain.Entities.Employee>().HasData(new Employee.Domain.Entities.Employee
            {
                Id = 2,
                Name = "Arturo"
            });

            modelBuilder.Entity<Permission>().HasData(new Permission
            {
                Id = 1,
                EmployeeId = 1,
                PermissionTypeId = 1
            });
        }
    }
}
