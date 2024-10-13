using MbaDevXpertBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbaDevXpertBlog.Data.Interfaces
{
    public interface IComentarioRepository : IRepository<Comentario>
    {
        Task<Comentario> GetCommentAuthorById(int id);
        Task<IEnumerable<Comentario>> GetCommentAuthorByPostId(int id);
        Task<IEnumerable<Comentario>> GetCommentByAuthorId(int id);
    }
}
