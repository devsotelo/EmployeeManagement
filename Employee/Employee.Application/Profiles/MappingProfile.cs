using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Employee.Application.Features.Permissions.Commands.CreatePermission;
using Employee.Application.Features.Permissions.Queries.GetPermissionsList;
using Employee.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Employee.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Permission, PermissionVm>().ReverseMap();
            CreateMap<Permission, CreatePermissionCommand>().ReverseMap();
        }
    }
}
