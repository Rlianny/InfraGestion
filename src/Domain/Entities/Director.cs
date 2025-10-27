using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Director : User
    {
        public Director(string fullName, string passwordHash, int departmentID) : base(fullName, passwordHash, departmentID) { }
        private Director() : base() { }
    }
}
