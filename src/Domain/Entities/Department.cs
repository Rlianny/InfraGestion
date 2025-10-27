namespace Domain.Entities
{
    public class Department
    {
        public int DepartmentID { get; private set; }
        public int SectionID { get; private set; }
        public string Name { get; private set; }
        public Department(string name,int sectionID)
        {
            Name = name;
            SectionID = sectionID;
        }
        private Department()
        {
            Name = String.Empty;
        }
    }
}