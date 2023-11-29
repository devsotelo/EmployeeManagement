namespace Employee.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int PermissionTypeId { get; set; }

        public Employee Employee { get; set; } = default!;

        public PermissionType PermissionType { get; set; } = default!;
    }
}
