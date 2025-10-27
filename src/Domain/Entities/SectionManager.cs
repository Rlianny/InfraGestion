using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SectionManager : User
    {
        public SectionManager(string fullName, string passwordHash, int sectionID) : base(fullName, passwordHash)
        {
            SectionID = sectionID;
        }

        public int SectionID { get; private set; }

        public virtual Section? Section { get; private set; }
    }
}
