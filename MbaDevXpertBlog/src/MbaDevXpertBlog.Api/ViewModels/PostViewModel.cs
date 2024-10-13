using MbaDevXpertBlog.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MbaDevXpertBlog.Api.ViewModels
{
    public class PostViewModel
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public int AutorId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; }
        [Display(Name = "Criado Em")]
        public DateTime DateCreated { get; set; }

        /* EF Relations */
        public Autor? Autor { get; set; }
        public IEnumerable<Comentario>? Comentarios { get; set; }
    }
    public class PostDetailsViewModel
    {
        public PostViewModel? PostViewModel { get; set; }
        public IEnumerable<ComentarioViewModel>? ComentarioViewModel { get; set; }
    }
}

