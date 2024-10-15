using MbaDevXpertBlog.Data.Interfaces;
using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MbaDevXpertBlog.Data.Repository
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(MbaDevXpertBlogDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Autor>> GetAllWhereUserId(Guid id)
        {
            return await Db.Autores.AsNoTracking()
                   .Where(a => a.Id == id)
                   .ToListAsync();
        }

    }
}
