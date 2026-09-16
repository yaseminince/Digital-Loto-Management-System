using LOTO.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Loto> Lotos { get; set; }
    }
}
