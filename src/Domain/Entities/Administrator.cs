using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Administrator : User
    {
        public Administrator(string fullName, string passwordHash, Department department) : base(fullName, passwordHash, department) { }
        private Administrator() : base() { }
    }
}
