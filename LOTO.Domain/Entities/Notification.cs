using LOTO.Domain.Base;
using LOTO.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Domain.Entities
{
    public class Notification : BaseEntity
    {
            
        public int UserId { get; set; }
        public User User { get; set; }

        public int? LotoId { get; set; }
        public Loto? Loto { get; set; }

        public NotificationType Type { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadDate { get; set; }
    
}
}
