using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
        public class DeviceReceiver : User
        {
                public DeviceReceiver(string fullName, string passwordHash, int departmentID) : base(fullName, passwordHash, departmentID)
                {
                }
                private DeviceReceiver() : base()
                {
                }
        }
}
