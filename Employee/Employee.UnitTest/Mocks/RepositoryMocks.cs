using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Persistence;
using Employee.Domain.Entities;
using Moq;

namespace Employee.UnitTest.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<IEmployeeRepository> GetEmployeeRepository()
        {
            var employees = new List<Employee.Domain.Entities.Employee>
                {
                new Employee.Domain.Entities.Employee
                {
                    Id = 1,
                    Name = "Francisco"
                },
                new Employee.Domain.Entities.Employee
                {
                    Id = 2,
                    Name = "Arturo"
                }
            };

            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock.Setup(repo => repo.ListAllAsync()).ReturnsAsync(employees);

            repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Employee.Domain.Entities.Employee>())).ReturnsAsync(
                (Employee.Domain.Entities.Employee employee) =>
                {
                    employees.Add(employee);
                    return employee;
                });

            repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
                (int id) =>
                {
                    var permissionType = employees.SingleOrDefault(x => x.Id == id);
                    return permissionType;
                });

            return repositoryMock;
        }

        public static Mock<IPermissionTypeRepository> GetPermissionTypeRepository()
        {
            var permissionTypes = new List<PermissionType>
            {
                new PermissionType
                {
                    Id = 1,
                    Name = "READ"
                },
                new PermissionType
                {
                    Id = 2,
                    Name = "WRITE"
                },
                new PermissionType
                {
                    Id = 3,
                    Name = "EXECUTE"
                }
            };

            var repositoryMock = new Mock<IPermissionTypeRepository>();
            repositoryMock.Setup(repo => repo.ListAllAsync()).ReturnsAsync(permissionTypes);

            repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<PermissionType>())).ReturnsAsync(
                (PermissionType permission) =>
                {
                    permissionTypes.Add(permission);
                    return permission;
                });

            repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
                (int id) =>
                {
                    var permissionType = permissionTypes.SingleOrDefault(x=> x.Id == id);
                    return permissionType;
                });

            return repositoryMock;
        }

        public static Mock<IPermissionRepository> GetPermissionRepository()
        {
            var permissions = new List<Permission>
            {
                new Permission
                {
                    Id = 1,
                    EmployeeId = 1,
                    PermissionTypeId = 1
                }
            };

            var mockCategoryRepository = new Mock<IPermissionRepository>();
            mockCategoryRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(permissions);

            mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Permission>())).ReturnsAsync(
                (Permission permission) =>
                {
                    permissions.Add(permission);
                    return permission;
                });

            mockCategoryRepository.Setup(repo => repo.IsPermissionUnique(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(
                (int idEmployee, int idPermissionType) =>
                {
                    return permissions.SingleOrDefault(x => x.EmployeeId == idEmployee && x.PermissionTypeId == idPermissionType) != null;
                });

            return mockCategoryRepository;
        }
    }
}
