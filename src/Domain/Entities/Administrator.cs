using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Administrator : User
    {
        public Administrator(string fullName, string passwordHash, int departmentId) : base(fullName, passwordHash, departmentId) { }
        private Administrator() : base() { }

        public bool CanModifyAllRecords()
        {
            return IsActive;
        }
    }
}
