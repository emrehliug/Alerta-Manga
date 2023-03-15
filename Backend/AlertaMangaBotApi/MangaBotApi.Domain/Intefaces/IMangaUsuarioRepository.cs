using MangaBotApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaBotApi.Domain.Intefaces
{
    public interface IMangaUsuarioRepository
    {
        List<MangaUsuario> GetAllMangaUsuario();
        int Add(MangaUsuario obj);
        int Delete(MangaUsuario obj);
    }
}
