namespace Domain.Entities
{
    public class Department
    {
        public Guid DepartmentID { get; set; }
        public Guid SectionID { get; set; }
        public virtual Section? Section { get; set; }
    }
}