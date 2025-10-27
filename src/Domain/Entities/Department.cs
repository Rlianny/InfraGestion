namespace Domain.Entities
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public int SectionID { get; set; }
        public virtual Section? Section { get; set; }
    }
}