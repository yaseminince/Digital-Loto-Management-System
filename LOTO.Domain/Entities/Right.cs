using LOTO.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Domain.Entities
{
    public class Right : BaseEntity
    {
        public string Name { get; set; } // yetkinin ismi (CreateLoto gibi)
        public ICollection<Role> Roles { get; set; } // bu yetkiye sahip roller
    }
}
