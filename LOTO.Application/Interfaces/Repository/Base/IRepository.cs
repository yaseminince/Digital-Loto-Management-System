using LOTO.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Interfaces.Repository.Base
{
    public interface IRepository<TEntity>
          where TEntity : class, IEntity, new()
    {
        TEntity Get(Expression<Func<TEntity, bool>> filter = null);

        List<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter = null);

        Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> filter = null);

        TEntity Add(TEntity entity);

        Task<TEntity> AddAsync(TEntity entity);

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task Hardelete(TEntity entity);
    }
}
