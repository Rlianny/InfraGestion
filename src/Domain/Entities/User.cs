using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public User(string fullName, string passwordHash, Department department)
        {
            FullName = fullName;
            PasswordHash = passwordHash;
            Department = department;
            DepartmentID = department.DepartmentID;
        }
        protected User()
        {
            FullName = string.Empty;
            PasswordHash = string.Empty;
        }

        public int UserID { get; private set; }
        public string FullName { get; private set; }
        public string PasswordHash { get; private set; }
        public int DepartmentID { get; private set; }
    }
}
