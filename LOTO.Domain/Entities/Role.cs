using LOTO.Domain.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        // identityden id, name, normalizedname alır
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Right> Rights { get; set; }
    }
}
