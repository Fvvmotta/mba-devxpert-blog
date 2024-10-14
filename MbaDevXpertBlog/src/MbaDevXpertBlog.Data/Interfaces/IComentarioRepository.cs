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
        Task<Comentario> GetCommentAuthorById(Guid id);
        Task<IEnumerable<Comentario>> GetCommentAuthorByPostId(Guid id);
        Task<IEnumerable<Comentario>> GetCommentByAuthorId(Guid id);
    }
}
