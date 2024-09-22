using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbaDevXpertBlog.Data.Models
{
    public class Post : Entitidade
    {
        public int AutorId { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
    }
}
