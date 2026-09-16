using LOTO.Application.Interfaces.Repository;
using LOTO.Domain.Entities;
using LOTO.Infrastructure.EFCore;
using LOTO.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Infrastructure.Repository
{
    public class LocationRepository : EFRepository<Location>, ILocationRepository
    {
        public LocationRepository(LotoContext context) : base(context)
        {
        }
    }
}
