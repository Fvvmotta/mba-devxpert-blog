using AutoMapper;
using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Interfaces;
using MbaDevXpertBlog.Data.Models;
using MbaDevXpertBlog.Mvc.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace MbaDevXpertBlog.Mvc.Controllers
{
    [Authorize]
    [Route("comentarios")]
    public class ComentariosController : Controller
    {
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IPostRepository _postRepository;
        private readonly IAutorRepository _autorRepository;
        private readonly IMapper _mapper;
        private readonly MbaDevXpertBlogDbContext _context;
        public ComentariosController(IComentarioRepository comentarioRepository,
                                      IPostRepository postRepository,
                                      IAutorRepository autorRepository,
                                      IMapper mapper,
                                      MbaDevXpertBlogDbContext context)
        {
            _comentarioRepository = comentarioRepository;
            _postRepository = postRepository;
            _autorRepository = autorRepository;
            _mapper = mapper;
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        [Route("listall")]
        public async Task<IActionResult> IndexAll()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            return View("Index", _mapper.Map<IEnumerable<ComentarioViewModel>>(await _comentarioRepository.GetAll()));
        }
        public async Task<ActionResult> Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var autor = await _autorRepository.GetByIdentityId(userId);
            if (autor == null)
            {
                TempData["Error"] = "É necessário criar um autor antes de ver seus os comentários.";
                return RedirectToAction("Index", "Home");
            }
            var comentarios = await _comentarioRepository.GetCommentByAuthorId(autor.Id);
            return View(_mapper.Map<IEnumerable<ComentarioViewModel>>(comentarios));
        }
        [Route("{id:int}/detalhes")]
        public async Task<ActionResult> Details(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.GetCommentAuthorById(id));
            if (comentario == null || (userId != comentario.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            return View(comentario);
        }

        [HttpPost("novo")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int postId, ComentarioViewModel comentarioViewModel)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var autorId = await _autorRepository.GetByIdentityId(userId);
            if (ModelState.IsValid)
            {
                comentarioViewModel.AutorId = autorId.Id;
                await _comentarioRepository.Add(_mapper.Map<Comentario>(comentarioViewModel));
                TempData["Success"] = "Comentário cadastrado com sucesso.";
                return RedirectToAction("Details", "Posts", new { id = postId });
            }
            return View(comentarioViewModel);
        }

        [Route("editar/{id:int}")]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.GetCommentAuthorById(id));
            if (comentario == null || (userId != comentario.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            if (comentario == null)
            {
                return NotFound();
            }
            return View(comentario);
        }

        [HttpPost("editar/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ComentarioViewModel comentarioViewModel)
        {
            if (id != comentarioViewModel.Id)
            {
                return NotFound();
            }
            var userId = Guid.Parse(User.Identity.GetUserId());
            var comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.GetCommentAuthorById(id));
            if (comentario == null || (userId != comentario.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _comentarioRepository.Update(_mapper.Map<Comentario>(comentarioViewModel));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                TempData["Success"] = "Comentário editado com sucesso.";

                return RedirectToAction(nameof(Index));
            }
            return View(comentarioViewModel);
        }

        [Route("excluir/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.GetCommentAuthorById(id));
            if (comentario == null || (userId != comentario.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            return View(comentario);
        }

        [HttpPost("excluir/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.GetCommentAuthorById(id));
            if (comentario == null || (userId != comentario.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            if (comentario != null)
            {
                await _comentarioRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}