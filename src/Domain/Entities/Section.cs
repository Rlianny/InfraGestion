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
        public Section(string name)
        {
            Name = name;
        }
        private Section()
        {
            Name = String.Empty;
        }

        public int SectionID { get; private set; }
        public string Name { get; private set; }

    }
}
