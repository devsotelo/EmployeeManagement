using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Employee.Application.Features.Permissions.Queries.GetPermissionsList
{
    public class GetPermissionListQuery : IRequest<List<PermissionVm>>
    {
    }
}
