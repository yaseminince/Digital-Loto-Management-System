using LOTO.Application.DTO.Response;
using LOTO.Application.Interfaces.Repository.Base;
using LOTO.Domain.Entities;
using LOTO.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Interfaces.Repository
{
    public interface ILotoRepository : IRepository<Loto>
    {
        string GetNextLotoNumber(); // lotonun numarası
        Task<List<Loto>> GetListWithDetailsAsync();
        Task<LotoFilterResponse> GetFilteredAsync(List<LotoStatus> statuses,List<int> userIds,List<LotoType> lotoTypes,
    DateTime? startDateFrom,DateTime? startDateTo,string searchValue,string sortColumn,string sortDirection,int page,int pageSize); // filtreleme için
    }
}
