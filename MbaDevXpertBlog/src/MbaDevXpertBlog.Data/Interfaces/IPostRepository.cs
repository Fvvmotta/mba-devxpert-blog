using MbaDevXpertBlog.Data.Models;

namespace MbaDevXpertBlog.Data.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetAllPostAuthor();
        Task<IEnumerable<Post>> GetAllPostsByAuthorId(int id);
        Task<Post> GetPostAuthorCommentsById(int id);
    }
}

