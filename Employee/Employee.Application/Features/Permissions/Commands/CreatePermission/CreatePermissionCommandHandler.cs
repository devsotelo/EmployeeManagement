using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Employee.Application.Contracts.Infrastructure;
using Employee.Application.Contracts.Persistence;
using Employee.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Employee.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, int>
    {
        private readonly IPermissionRepository permissionRepository;
        private readonly IPermissionTypeRepository permissionTypeRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;
        private readonly IElasticService elasticService;
        private readonly IKafkaService kafkaService;
        private readonly ILogger<CreatePermissionCommandHandler> logger;

        public CreatePermissionCommandHandler(IMapper mapper, IPermissionRepository permissionRepository, IElasticService elasticService, IKafkaService kafkaService, ILogger<CreatePermissionCommandHandler> logger, IPermissionTypeRepository permissionTypeRepository, IEmployeeRepository employeeRepository)
        {
            this.mapper = mapper;
            this.permissionRepository = permissionRepository;
            this.elasticService = elasticService;
            this.logger = logger;
            this.kafkaService = kafkaService;
            this.permissionTypeRepository = permissionTypeRepository;
            this.employeeRepository = employeeRepository;
        }

        public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreatePermissionCommandValidator(permissionRepository, permissionTypeRepository, employeeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var permission = mapper.Map<Permission>(request);

            permission = await permissionRepository.AddAsync(permission);

            try
            {
                await elasticService.Send(new Models.PermissionDocument
                {
                    Id = permission.Id,
                    EmployeeId = permission.EmployeeId,
                    PermissionTypeId = permission.PermissionTypeId
                });
            }
            catch (Exception ex)
            {
                logger.LogError($"Elastic for {permission.Id} failed due to an error with the service: {ex.Message}");
            }

            try
            {
                await kafkaService.Send(new Models.OperationMessage
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "request"

                });
            }
            catch (Exception ex)
            {
                logger.LogError($"Kafka for {permission.Id} failed due to an error with the service: {ex.Message}");
            }

            return permission.Id;
        }
    }
}
