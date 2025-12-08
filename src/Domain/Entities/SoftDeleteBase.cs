using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Domain.Entities
{
    public abstract class SoftDeleteBase
    {
        public abstract bool IsDisabled { get; set; }
        public void Disable()
        { 
          IsDisabled = true;
        }
    }
}
