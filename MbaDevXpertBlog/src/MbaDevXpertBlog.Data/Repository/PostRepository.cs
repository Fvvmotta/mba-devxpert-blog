using MbaDevXpertBlog.Data.Interfaces;
using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MbaDevXpertBlog.Data.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(MbaDevXpertBlogDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Post>> GetAllPostAuthor()
        {
            return await Db.Posts.AsNoTracking()
                   .Include(p => p.Autor)
                   .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPostsByAuthorId(Guid id)
        {
            return await Db.Posts.AsNoTracking()
                   .Where(p => p.AutorId == id)
                   .ToListAsync();
        }

        public async Task<Post> GetPostAuthorCommentsById(Guid id)
        {
            return await Db.Posts.AsNoTracking()
                    .Include(p => p.Autor)
                    .Include(p => p.Comentarios)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
