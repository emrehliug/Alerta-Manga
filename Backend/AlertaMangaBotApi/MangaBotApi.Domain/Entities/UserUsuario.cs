using MangaBotApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaBotApi.Domain.Entities
{
    public class UserUsuario
    {
        public User User { get; set; }
        public Usuario Usuario { get; set; }
        public List<Manga> Manga { get; set; }
    }
}
