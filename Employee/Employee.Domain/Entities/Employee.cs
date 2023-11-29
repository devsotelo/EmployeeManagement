namespace Employee.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Permission>? Permissions { get; set; }
    }
}
