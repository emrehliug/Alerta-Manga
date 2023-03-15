using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaBotApi.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        public string numero_phone { get; set; }

        public string nomecompleto { get; set; }

        public string email { get; set; }

        public string apikey { get; set; }

    }
}
