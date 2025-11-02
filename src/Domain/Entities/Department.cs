namespace Domain.Entities
{
    public class Department
    {
        public Department(string name, int sectionID)
        {
            Name = name;
            SectionID = sectionID;
        }

        private Department() { }

        public int DepartmentID { get; private set; }
        public string Name { get; private set; } = String.Empty;

        // Navigation properties
        public int SectionID { get; private set; }
        public Section? Section { get; private set; }
        public ICollection<Device> Device { get; set; } = new List<Device>();
        public ICollection<User> Staff { get; set; } = new List<User>();
    }
}
