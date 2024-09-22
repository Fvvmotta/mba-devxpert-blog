using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbaDevXpertBlog.Data.Models
{
    public class Comentario : Entitidade
    {
        public int PostId { get; set; }
        public int AutorId { get; set; }
        public int Conteudo { get; set; }
    }
}
