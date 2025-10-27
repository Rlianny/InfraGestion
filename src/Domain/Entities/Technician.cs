using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Technician:User
    {
        public Technician(string fullName, string passwordHash, int yearsOfExperience, string specialty)
            : base(fullName, passwordHash)
        {
            YearsOfExperience = yearsOfExperience;
            Specialty = specialty;
        }

        public int YearsOfExperience { get;private set; }  
        public string Specialty { get;private set; }

    }
}
