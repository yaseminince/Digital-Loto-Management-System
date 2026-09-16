using LOTO.Application.DTO.Request;
using LOTO.Application.DTO.Response;
using LOTO.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Interfaces
{
    public interface ILotoService
    {
        LotoResponse GetById(int id);
        LotoResponse GetByFilter(LotoRequest request);
        Task<LotoResponse> GetListAsync();
        LotoResponse Add(LotoRequest request);
        Task<LotoResponse> AddAsync(LotoRequest request);
        Task<LotoResponse> CloseAsync(int lotoId);
        void Delete(LotoRequest request);
        Task DeleteAsync(LotoRequest request);

        Task<LotoFilterResponse> GetFilteredAsync(List<LotoStatus> statuses,List<int> userIds,List<LotoType> lotoTypes,DateTime? startDateFrom,
    DateTime? startDateTo,string searchValue,string sortColumn, string sortDirection, int page,int pageSize);
    }
}
