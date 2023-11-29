using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Employee.Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommand : IRequest
    {
        public int Id { get; set; }

        public int IdEmployee { get; set; }

        public int IdPermissionType { get; set; }
    }
}
