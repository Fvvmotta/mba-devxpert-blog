namespace MbaDevXpertBlog.Data.Models
{
    public class Autor : Entity
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreated { get; set; }

        /* EF Relations */
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Comentario> Comentarios { get; set; }
    }
}

