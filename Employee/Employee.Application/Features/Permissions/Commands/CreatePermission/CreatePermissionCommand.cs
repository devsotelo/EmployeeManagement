using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Employee.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommand : IRequest<int>
    {
        public int EmployeeId { get; set; }

        public int PermissionTypeId { get; set; }
    }
}
