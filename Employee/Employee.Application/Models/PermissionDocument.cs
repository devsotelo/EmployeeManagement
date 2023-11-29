using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Models
{
    public class PermissionDocument
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int PermissionTypeId { get; set; }
    }
}
