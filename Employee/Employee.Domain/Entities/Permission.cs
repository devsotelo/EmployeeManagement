namespace Employee.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }

        public int IdEmployee { get; set; }

        public int IdPermissionType { get; set; }

        public Employee Employee { get; set; } = default!;

        public PermissionType PermissionType { get; set; } = default!;
    }
}
