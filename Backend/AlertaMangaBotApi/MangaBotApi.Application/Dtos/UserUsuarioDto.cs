using MangaBotApi.Domain.Entities.Identity;
using MangaBotApi.Domain.Entities;

namespace MangaBotApi.Application.Dtos
{
    public class UserUsuarioDto
    {
        public UserDto User { get; set; }
        public Usuario Usuario { get; set; }
        public List<Manga> Manga { get; set; }
    }
}
