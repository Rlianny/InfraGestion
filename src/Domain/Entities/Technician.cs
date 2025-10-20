using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Technician:User
    {
        public Technician(string fullName, string passwordHash, Guid departmentID, int yearsOfExperience, string specialty)
            : base(fullName, passwordHash, departmentID)
        {
            YearsOfExperience = yearsOfExperience;
            Specialty = specialty;
        }

        public int YearsOfExperience { get; set; }  
        public string Specialty { get; set; }

    }
}
