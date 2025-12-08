using Domain.Interfaces;

namespace Domain.Entities
{
    public class Department:SoftDeleteBase
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
        public override bool IsDisabled { get; set; } = false;

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del departamento no puede estar vacío", nameof(name));
            Name = name;
        }

        public void UpdateSection(int sectionId)
        {
            SectionId = sectionId;
        }
    }
}
