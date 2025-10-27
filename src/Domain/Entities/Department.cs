namespace Domain.Entities
{
    public class Department
    {
        public int DepartmentID { get; private set; }
        public int SectionID { get; private set; }
        public virtual Section? Section { get; private set; }
        private Department()
        {
        }
    }
}