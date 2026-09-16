using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Interfaces
{
    public interface IGenericService<TResponse, TRequest>
    {
        TResponse GetById(int id);
        TResponse GetByFilter(TRequest request);
        TResponse GetList();
        TResponse Add(TRequest request);
        Task<TResponse> AddAsync(TRequest request);
        TResponse Update(TRequest request);
        Task<TResponse> UpdateAsync(TRequest request);
        void Delete(TRequest request);
        Task DeleteAsync(TRequest request);
    }
}
