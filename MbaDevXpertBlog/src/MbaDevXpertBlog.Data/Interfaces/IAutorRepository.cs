using MbaDevXpertBlog.Data.Models;

namespace MbaDevXpertBlog.Data.Interfaces
{
    public interface IAutorRepository : IRepository<Autor>
    {
        Task<IEnumerable<Autor>> GetAllWhereUserId(Guid id);
    }
}
