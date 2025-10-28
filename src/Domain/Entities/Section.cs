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
            ValidateName(name);
            Name = name;
        }
        private Section()
        {
        }
        public string Name { get; private set; }
        public int SectionID { get; private set; }
        private void ValidateName(string name)
        {
            if (name == string.Empty || name.Length < 3)
            {
                throw new ArgumentException("Name to short");
            }
        }

    }
}
