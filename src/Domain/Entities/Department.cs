namespace Domain.Entities
{
    public class Department
    {
        public Department(string name, int sectionId)
        {
            Name = name;
            SectionId = sectionId;
        }

        private Department() { }

        public int DepartmentId { get; private set; }
        public string Name { get; private set; } = String.Empty;

        // Navigation properties
        public int SectionId { get; private set; }
        public Section? Section { get; private set; }
        public ICollection<Device> Device { get; set; } = new List<Device>();
        public ICollection<User> Staff { get; set; } = new List<User>();
        public ICollection<Department> Departments { get; private set; }
    }
}
