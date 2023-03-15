using MangaBotApi.Domain.Intefaces;
using MangaBotApi.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaBotApi.Infra.Data.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly MySqlContext _mySqlContext;

        public BaseRepository(MySqlContext mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _mySqlContext.Set<TEntity>().AsNoTracking();
        }

        public int Add(TEntity obj)
        {
            _mySqlContext.Set<TEntity>().Add(obj);
            return _mySqlContext.SaveChanges();
        }
        public int Update(TEntity obj)
        {
            _mySqlContext.Entry(obj).State = EntityState.Modified;
            return _mySqlContext.SaveChanges();
        }
        public int Delete(int id)
        {
            _mySqlContext.Set<TEntity>().Remove(Get(id));
            return _mySqlContext.SaveChanges();
        }

        public TEntity Get(int id) =>
            _mySqlContext.Set<TEntity>().Find(id);

        public TEntity Get(Func<TEntity, bool> predicate) =>
            _mySqlContext.Set<TEntity>().Where(predicate).FirstOrDefault();

        public IList<TEntity> GetAll() =>
            _mySqlContext.Set<TEntity>().AsNoTracking().ToList();

        public IQueryable<TEntity> GetAll(Func<TEntity, bool> predicate) =>
             (IQueryable<TEntity>)_mySqlContext.Set<TEntity>().Where(predicate);

    }
}
