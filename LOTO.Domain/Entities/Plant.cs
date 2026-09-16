using LOTO.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Domain.Entities
{
    public class Plant : BaseEntity
    {
        public string Name { get; set; } // plantin adı

        public bool IsActive { get; set; }

        public ICollection<Department> Departments { get; set; }

        public ICollection<Loto> Lotos { get; set; }
    }
}
