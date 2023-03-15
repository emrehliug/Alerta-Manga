using MangaBotApi.Domain.Entities;
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
    public class MangaUsuarioRepository : IMangaUsuarioRepository
    {
        protected readonly MySqlContext _mySqlContext;

        public MangaUsuarioRepository(MySqlContext mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _mySqlContext.Set<MangaUsuario>().AsNoTracking();
        }

        public List<MangaUsuario> GetAllMangaUsuario() 
        {
            IQueryable<MangaUsuario> query = this._mySqlContext.tbMangaUsuario
                .Include(m => m.Manga)
                .Include(u => u.Usuario);
            return query.ToList();
        }

        public int Add(MangaUsuario obj)
        {
            _mySqlContext.Set<MangaUsuario>().Add(obj);
            return _mySqlContext.SaveChanges();
        }

        public int Delete(MangaUsuario mangaUsuario)
        {
            _mySqlContext.Set<MangaUsuario>().Remove(mangaUsuario);
            return _mySqlContext.SaveChanges();
        }
    }
}
