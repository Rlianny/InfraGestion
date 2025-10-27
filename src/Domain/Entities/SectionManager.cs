using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SectionManager : User
    {
        public SectionManager(string fullName, string passwordHash,int departmentID, int sectionID) : base(fullName, passwordHash,departmentID)
        {
            SectionID = sectionID;
        }
        private SectionManager() : base()
        {
        }
        public int SectionID { get; private set; }

    }
}
