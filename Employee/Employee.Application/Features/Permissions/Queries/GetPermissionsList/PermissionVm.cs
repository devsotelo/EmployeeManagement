using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Domain.Entities;

namespace Employee.Application.Features.Permissions.Queries.GetPermissionsList
{
    public class PermissionVm
    {
        public int Id { get; set; }

        public int IdEmployee { get; set; }

        public int IdPermissionType { get; set; }

        public string Employee { get; set; } = string.Empty;

        public string PermissionType { get; set; } = string.Empty;
    }
}
