using Domain.Aggregations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Section
    {
        public Section(int sectionID, string name)
        {
            SectionID = sectionID;
            Name = name;
        }
        private Section()
        {
        }

        public int SectionID { get; private set; }
        public string Name { get; private set; }

    }
}
