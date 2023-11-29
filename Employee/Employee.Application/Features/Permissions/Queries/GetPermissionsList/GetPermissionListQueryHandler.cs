using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Employee.Application.Contracts.Persistence;
using Employee.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Employee.Application.Features.Permissions.Queries.GetPermissionsList
{
    public class GetPermissionListQueryHandler : IRequestHandler<GetPermissionListQuery, List<PermissionVm>>
    {
        private readonly IAsyncRepository<Permission> permissionRepository;
        private readonly IMapper mapper;

        public GetPermissionListQueryHandler(IMapper mapper, IAsyncRepository<Permission> permissionRepository)
        {
            this.mapper = mapper;
            this.permissionRepository = permissionRepository;
        }

        public async Task<List<PermissionVm>> Handle(GetPermissionListQuery request, CancellationToken cancellationToken)
        {
            var allEvents = (await permissionRepository.ListAllAsync()).OrderBy(x => x.Id);
            return mapper.Map<List<PermissionVm>>(allEvents);
        }
    }
}
