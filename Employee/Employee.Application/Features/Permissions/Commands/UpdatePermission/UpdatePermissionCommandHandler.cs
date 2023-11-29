using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Employee.Application.Contracts.Infrastructure;
using Employee.Application.Contracts.Persistence;
using Employee.Application.Exceptions;
using Employee.Application.Features.Permissions.Commands.CreatePermission;
using Employee.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Employee.Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand>
    {
        private readonly IPermissionRepository permissionRepository;
        private readonly IPermissionTypeRepository permissionTypeRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;
        private readonly IElasticService elasticService;
        private readonly IKafkaService kafkaService;
        private readonly ILogger<UpdatePermissionCommandHandler> logger;

        public UpdatePermissionCommandHandler(IMapper mapper, IPermissionRepository permissionRepository, IElasticService elasticService, IKafkaService kafkaService, ILogger<UpdatePermissionCommandHandler> logger, IPermissionTypeRepository permissionTypeRepository, IEmployeeRepository employeeRepository)
        {
            this.mapper = mapper;
            this.permissionRepository = permissionRepository;
            this.elasticService = elasticService;
            this.logger = logger;
            this.kafkaService = kafkaService;
            this.permissionTypeRepository = permissionTypeRepository;
            this.employeeRepository = employeeRepository;
        }

        public async Task Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await permissionRepository.GetByIdAsync(request.Id);
            if (permission == null)
            {
                throw new NotFoundException(nameof(Permission), request.Id);
            }

            var validator = new UpdatePermissionCommandValidator(permissionRepository, permissionTypeRepository, employeeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            mapper.Map(request, permission, typeof(UpdatePermissionCommand), typeof(Permission));

            await permissionRepository.UpdateAsync(permission);

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
                    Name = "modify"
                });
            }
            catch (Exception ex)
            {
                logger.LogError($"Kafka for {permission.Id} failed due to an error with the service: {ex.Message}");
            }

            return;
        }
    }
}
