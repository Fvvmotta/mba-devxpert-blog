using MbaDevXpertBlog.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MbaDevXpertBlog.Api.ViewModels
{
    public class ComentarioViewModel
    {
        [Key]
        public Guid? Id { get; set; }
        public Guid? PostId { get; set; }
        public Guid? AutorId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; }
        [Display(Name = "Criado Em")]
        public DateTime? DateCreated { get; set; }

        /* EF Relations */
        public AutorViewModel? Autor { get; set; }
        public PostViewModel? PostViewMode { get; set; }
    }
}
