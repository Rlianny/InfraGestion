using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EquipmentReceiver : User
    {
        public EquipmentReceiver(string fullName, string passwordHash) : base(fullName, passwordHash)
        {
        }
    }
}
