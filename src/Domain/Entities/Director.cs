using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Director : User
    {
        public Director(string fullName, string passwordHash, Department department) : base(fullName, passwordHash, department) { }
        private Director() : base() { }
    }
}
