using MbaDevXpertBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MbaDevXpertBlog.Api.ViewModels
{
    public class AutorViewModel
    {
        public Guid? Id { get; set; }
        public Guid? IdentityUserId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "E-mail")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "O campo {0} está em formato invalido")]
        public string Email { get; set; }

        /* EF Relations */
        public IEnumerable<PostViewModel>? PostViewModel { get; set; }
        public IEnumerable<ComentarioViewModel>? ComentarioViewModel { get; set; }

    }
}

