﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Employee.Application.Contracts.Infrastructure;
using Employee.Application.Contracts.Persistence;
using Employee.Application.Features.Permissions.Commands.UpdatePermission;
using Employee.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Employee.Application.Features.Permissions.Queries.GetPermissionsList
{
    public class GetPermissionListQueryHandler : IRequestHandler<GetPermissionListQuery, List<PermissionVm>>
    {
        private readonly IAsyncRepository<Permission> permissionRepository;
        private readonly IMapper mapper;
        private readonly IKafkaService kafkaService;
        private readonly ILogger<GetPermissionListQueryHandler> logger;

        public GetPermissionListQueryHandler(IMapper mapper, IAsyncRepository<Permission> permissionRepository, IKafkaService kafkaService, ILogger<GetPermissionListQueryHandler> logger)
        {
            this.mapper = mapper;
            this.permissionRepository = permissionRepository;
            this.kafkaService = kafkaService;
            this.logger = logger;
        }

        public async Task<List<PermissionVm>> Handle(GetPermissionListQuery request, CancellationToken cancellationToken)
        {
            var allPermissions = (await permissionRepository.ListAllAsync()).OrderBy(x => x.Id);
            return mapper.Map<List<PermissionVm>>(allPermissions);
        }
    }
}
