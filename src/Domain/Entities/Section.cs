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

        private Section() { }

        public string Name { get; private set; }
        public int SectionId { get; private set; }
        public int? SectionManagerId { get; private set; }
        public User? SectionManager { get; private set; }
        public void AssignManager(User manager)
        {
            SectionManager = manager;
            SectionManagerId = manager.UserId;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la sección no puede estar vacío", nameof(name));
            Name = name;
        }
    }
}
