using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbaDevXpertBlog.Data.Models
{
    public class Comentario : Entity
    {
        public Guid PostId { get; set; }
        public Guid AutorId { get; set; }
        public string Conteudo { get; set; }
        public DateTime DateCreated { get; set; }

        /* EF Relations */
        public Autor Autor { get; set; }
    }
}
