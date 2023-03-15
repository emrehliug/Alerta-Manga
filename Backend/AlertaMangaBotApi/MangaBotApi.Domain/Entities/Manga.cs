using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaBotApi.Domain.Entities
{
    public class Manga
    {
        [Key]
        public int MangaId { get; set; }

        public string Nome { get; set; }

        public int TotaldeCapitulos { get; set; }

    }
}
