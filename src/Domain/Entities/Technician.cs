using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Technician:User
    {
        public Technician(string fullName, string passwordHash, Department department, int yearsOfExperience, string specialty)
            : base(fullName, passwordHash, department)
        {
            YearsOfExperience = yearsOfExperience;
            Specialty = specialty;
        }
        private Technician() : base()
        {
            Specialty = string.Empty;
        }

        public int YearsOfExperience { get; private set; }  
        public string Specialty { get;private set; }

    }
}
