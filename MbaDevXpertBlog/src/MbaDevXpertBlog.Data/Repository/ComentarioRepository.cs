using MbaDevXpertBlog.Data.Interfaces;
using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MbaDevXpertBlog.Data.Repository
{
    public class ComentarioRepository : Repository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(MbaDevXpertBlogDbContext db) : base(db)
        {
        }
        public async Task<Comentario> GetCommentAuthorById(Guid id)
        {
            return await Db.Comentarios.AsNoTracking()
                    .Include(p => p.Autor)
                    .FirstAsync();
        }
        public async Task<IEnumerable<Comentario>> GetCommentAuthorByPostId(Guid id)
        {
            return await Db.Comentarios.AsNoTracking()
                    .Include(p => p.Autor)
                    .Where(p => p.PostId == id)
                    .ToListAsync();
        }
        public async Task<IEnumerable<Comentario>> GetCommentByAuthorId(Guid id)
        {
            return await Db.Comentarios.AsNoTracking()
                    .Include(p => p.Autor)
                    .Where(p => p.AutorId == id)
                    .ToListAsync();
        }
    }
}
