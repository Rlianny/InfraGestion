using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class SectionManager : User
    {
        public int SectionID { get; private set; }
        List<int> ManagedSectionsIds = new List<int>();
        public SectionManager(string fullName, string passwordHash, int departmentID, int sectionID) : base(fullName, passwordHash, departmentID)
        {
            SectionID = sectionID;
        }
        private SectionManager() : base() { }

        public bool CanManageSection(int sectionID)
        {
            return ManagedSectionsIds.Contains(sectionID);
        }
    }
}
