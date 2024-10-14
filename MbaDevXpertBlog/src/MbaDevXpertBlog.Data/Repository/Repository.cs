using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MbaDevXpertBlog.Data.Interfaces;
using MbaDevXpertBlog.Data.Models;
using MbaDevXpertBlog.Data.Context;

namespace MbaDevXpertBlog.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly MbaDevXpertBlogDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(MbaDevXpertBlogDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await DbSet.AsNoTracking().Where(predicate).ToListAsync();
            return result;
        }
        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        public virtual async Task<List<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }
        public virtual async Task Add(TEntity entity)
        {
            try
            {
                DbSet.Add(entity);
            }
            catch (Exception ex)
            {
                string save = ex.Message;
            }
            await SaveChanges();
        }
        public virtual async Task Update(TEntity entity)
        {
            var update = DbSet.Update(entity);
            await SaveChanges();
        }
        public virtual async Task Delete(Guid id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChanges();
        }
        public async Task<int> SaveChanges()
        {
            try
            {
                var save = await Db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string save = ex.Message;
            }
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
