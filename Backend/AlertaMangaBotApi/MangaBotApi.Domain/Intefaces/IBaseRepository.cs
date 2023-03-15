using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaBotApi.Domain.Intefaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        int Add(TEntity obj);
        int Update(TEntity obj);
        int Delete(int id);
        TEntity Get(int id);
        TEntity Get(Func<TEntity, bool> predicate);
        IList<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Func<TEntity, bool> predicate);
    }
}
