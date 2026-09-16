using LOTO.Domain.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        // identity ile otomatik id , user , email , hashedpassword geliyo
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Loto> Lotos { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
