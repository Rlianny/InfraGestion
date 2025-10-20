using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public User(string fullName, string passwordHash, Guid departmentID)
        {
            UserID = Guid.NewGuid();
            FullName = fullName;
            PasswordHash = passwordHash;
            DepartmentID = departmentID;
        }

        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string PasswordHash {  get; set; }   
        public Guid DepartmentID {get;set;}

        public virtual Department? Department { get; set; }
    }
}
