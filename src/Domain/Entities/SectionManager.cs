using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SectionManager:User
    {
        public SectionManager(string fullName, string passwordHash, Guid departmentID, Guid sectionID) : base(fullName, passwordHash, departmentID)
        {
            SectionID = sectionID;
        }

        public Guid SectionID { get; set; }

        public virtual Section? Section { get; set; }
    }
}
