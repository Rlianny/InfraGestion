namespace Domain.Entities
{
    public class Department
    {
        public int DepartmentID { get; private set; }
        public int SectionID { get; private set; }
        public Department(int sectionID)
        {
            SectionID = sectionID;
        }
        private Department() { }
    }
}