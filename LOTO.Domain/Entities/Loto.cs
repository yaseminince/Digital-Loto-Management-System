using LOTO.Domain.Base;
using LOTO.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Domain.Entities
{
    public class Loto : BaseEntity
    {
        public string LotoNumber { get; set; } // kaçıncı loto

        public int UserId { get; set; } // oluşturan kişinin idsi
        public User User { get; set; } // oluşturan kişinin ismi

        public int PlantId { get; set; } 
        public Plant Plant { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        public LotoType LotoType { get; set; }
        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpectedEndDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public LotoStatus Status { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}
