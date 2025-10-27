using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SectionManager : User
    {
        public SectionManager(string fullName, string passwordHash, Department department, Section section) : base(fullName, passwordHash, department)
        {
            Section = section;
            SectionID = section.SectionID;
        }
        private SectionManager() : base()
        {
        }


        public int SectionID { get; private set; }

        public virtual Section? Section { get; private set; }
    }
}
