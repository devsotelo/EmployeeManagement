using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Employee.Application.Contracts.Infrastructure;
using Employee.Application.Contracts.Persistence;
using Employee.Application.Features.Permissions.Commands.CreatePermission;
using Employee.Application.Profiles;
using Employee.Domain.Entities;
using Employee.UnitTest.Mocks;
using EmptyFiles;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace Employee.UnitTest.Permissions.Commands
{
    public class CreatePermissionTest
    {
        private readonly IMapper mapper;
        private Mock<ILogger<CreatePermissionCommandHandler>> logger;
        private readonly Mock<IPermissionRepository> permissionRepository;
        private readonly Mock<IPermissionTypeRepository> permissionTypeRepository;

        public Mock<IEmployeeRepository> employeeRepository { get; }

        private readonly Mock<IKafkaService> kafkaService;
        private readonly Mock<IElasticService> elasticService;

        public CreatePermissionTest()
        {
            permissionRepository = RepositoryMocks.GetPermissionRepository();
            permissionTypeRepository = RepositoryMocks.GetPermissionTypeRepository();
            employeeRepository = RepositoryMocks.GetEmployeeRepository();

            kafkaService = KafkaMocks.GetKafkaService();
            elasticService = ElasticSearchMocks.GetElasticService();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            mapper = configurationProvider.CreateMapper();

            logger = new Mock<ILogger<CreatePermissionCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ValidPermission_AddedToRepo()
        {
            var handler = new CreatePermissionCommandHandler(
                mapper, 
                permissionRepository.Object,
                elasticService.Object,
                kafkaService.Object,
                logger.Object,
                permissionTypeRepository.Object,
                employeeRepository.Object
                );

            await handler.Handle(new CreatePermissionCommand() { EmployeeId = 2, PermissionTypeId = 1 }, CancellationToken.None);

            var allCategories = await permissionRepository.Object.ListAllAsync();
            allCategories.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_ValidPermission_EmployeeNotExists()
        {
            var handler = new CreatePermissionCommandHandler(
                mapper,
                permissionRepository.Object,
                elasticService.Object,
                kafkaService.Object,
                logger.Object,
                permissionTypeRepository.Object,
                employeeRepository.Object
                );


            var action = async () => await handler.Handle(new CreatePermissionCommand() { EmployeeId = 3, PermissionTypeId = 1 }, CancellationToken.None);
            var ex = await Assert.ThrowsAsync<Employee.Application.Exceptions.ValidationException>(() => action());
            ex.ShouldBeOfType<Employee.Application.Exceptions.ValidationException>();
        }
    }
}
