namespace Domain.Entities
{
    public class Department
    {
        public int DepartmentID { get; private set; }
        public int SectionID { get; private set; }
        public string Name { get; private set; }
        public virtual Section? Section { get; private set; }
        private Department()
        {
            Name = String.Empty;
        }
        public Department(string name, Section section)
        {
            Name = name;
            Section = section;
            SectionID = section.SectionID;
        }
    }
}