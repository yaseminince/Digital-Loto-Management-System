using LOTO.Application.DTO.Response;
using LOTO.Application.Interfaces.Repository;
using LOTO.Domain.Entities;
using LOTO.Domain.Enums;
using LOTO.Infrastructure.EFCore;
using LOTO.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Infrastructure.Repository
{
    public class LotoRepository : EFRepository<Loto>, ILotoRepository
    {
        public LotoRepository(LotoContext context) : base(context)
        {
        }
        public string GetNextLotoNumber()
        {
            var lastLoto = _context.Loto
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (lastLoto == null)
                return "LOTO-2026-0001";

            var lastNumber = int.Parse(
                lastLoto.LotoNumber.Split('-').Last());

            return $"LOTO-{DateTime.Now.Year}-{lastNumber + 1:D4}";
        }

        public async Task<List<Loto>> GetListWithDetailsAsync()
        {
            return await _context.Loto.Include(x => x.User).Include(x => x.Plant).Include(x => x.Department)
                .Include(x => x.Location).Include(x => x.Equipment).ToListAsync();
        }

        public async Task<LotoFilterResponse> GetFilteredAsync(List<LotoStatus> statuses,List<int> userIds,List<LotoType> lotoTypes,
    DateTime? startDateFrom,DateTime? startDateTo,string searchValue,string sortColumn,string sortDirection,int page,int pageSize)
        {
            var query = _context.Loto.Include(x => x.User).Include(x => x.Plant).Include(x => x.Department).Include(x => x.Location)
                .Include(x => x.Equipment).AsQueryable();

            if (statuses != null && statuses.Any())
            {
                query = query.Where(x =>statuses.Contains(x.Status));
            }

            if (userIds != null && userIds.Any())
            {
                query = query.Where(x =>userIds.Contains(x.UserId));
            }

            if (lotoTypes != null && lotoTypes.Any())
            {
                query = query.Where(x =>lotoTypes.Contains(x.LotoType));
            }

            if (startDateFrom.HasValue)
            {
                query = query.Where(x =>x.StartDate >= startDateFrom.Value);
            }

            if (startDateTo.HasValue)
            {
                var endDate = startDateTo.Value.Date.AddDays(1);

                query = query.Where(x =>x.StartDate < endDate);
            }

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                query = query.Where(x =>
                    x.LotoNumber.Contains(searchValue) ||
                    x.User.Name.Contains(searchValue) ||
                    x.Plant.Name.Contains(searchValue) ||
                    x.Department.Name.Contains(searchValue) ||
                    x.Location.Name.Contains(searchValue) ||
                    x.Equipment.Name.Contains(searchValue) ||
                    (x.Description != null && x.Description.Contains(searchValue)));
            }

            var totalCount = await query.CountAsync();

            // SORTING

            bool ascending =string.Equals(sortDirection,"asc", StringComparison.OrdinalIgnoreCase);

            switch (sortColumn)
            {
                case "lotoNumber":

                    query = ascending? query.OrderBy(x => x.LotoNumber): query.OrderByDescending(x => x.LotoNumber);
                    break;

                case "user":
                    query = ascending ? query.OrderBy(x => x.User.Name): query.OrderByDescending(x => x.User.Name);
                    break;

                case "plant":
                    query = ascending? query.OrderBy(x => x.Plant.Name) : query.OrderByDescending(x => x.Plant.Name);
                    break;

                case "department":
                    query = ascending? query.OrderBy(x => x.Department.Name): query.OrderByDescending(x => x.Department.Name);
                    break;

                case "location":
                    query = ascending? query.OrderBy(x => x.Location.Name)
                        : query.OrderByDescending(x => x.Location.Name);
                    break;

                case "equipment":
                    query = ascending ? query.OrderBy(x => x.Equipment.Name): query.OrderByDescending(x => x.Equipment.Name);
                    break;

                case "startDate":
                    query = ascending? query.OrderBy(x => x.StartDate): query.OrderByDescending(x => x.StartDate);
                    break;

                case "expectedEndDate":
                    query = ascending ? query.OrderBy(x => x.ExpectedEndDate): query.OrderByDescending(x => x.ExpectedEndDate);
                    break;

                case "endDate":
                    query = ascending? query.OrderBy(x => x.ClosedDate)
                        : query.OrderByDescending(x => x.ClosedDate);
                    break;

                case "status":
                    query = ascending? query.OrderBy(x => x.Status): query.OrderByDescending(x => x.Status);
                    break;

                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }


            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new LotoFilterResponse
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
