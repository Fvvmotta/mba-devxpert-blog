using AutoMapper;
using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Interfaces;
using MbaDevXpertBlog.Data.Models;
using MbaDevXpertBlog.Data.Repository;
using MbaDevXpertBlog.Mvc.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace MbaDevXpertBlog.Mvc.Controllers
{
    [Route("posts")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IAutorRepository _autorRepository;
        private readonly IMapper _mapper;
        private readonly MbaDevXpertBlogDbContext _context;
        public PostsController(IPostRepository postRepository,
                               IComentarioRepository comentarioRepository,
                               IAutorRepository autorRepository,
                               IMapper mapper,
                               MbaDevXpertBlogDbContext context)
        {
            _postRepository = postRepository;
            _comentarioRepository = comentarioRepository;
            _autorRepository = autorRepository;
            _mapper = mapper;
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        [Route("listall")]
        public async Task<IActionResult> IndexAll()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            return View("Index", _mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.GetAll()));
        }
        public async Task<ActionResult> Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var autor = await _autorRepository.GetByIdentityId(userId);
            if (autor == null)
            {
                TempData["Error"] = "É necessário criar um autor antes de ver seus posts.";
                return RedirectToAction("Index", "Home");
            }
            return View(_mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.GetAllPostsByAuthorId(autor.Id)));
        }
        [AllowAnonymous]
        [Route("{id:Guid}/detalhes")]
        public async Task<ActionResult> Details(Guid id)
        {
            var post = _mapper.Map<PostViewModel>(await _postRepository.GetPostAuthorCommentsById(id));
            if (post == null)
            {
                return NotFound();
            }
            var comentarios = _mapper.Map<IEnumerable<ComentarioViewModel>>(await _comentarioRepository.GetCommentAuthorByPostId(id));
            var postDetailsViewModel = new PostDetailsViewModel
            {
                PostViewModel = post,
                ComentarioViewModel = comentarios
            };
            return View(postDetailsViewModel);
        }
        [Route("novo")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost("novo")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PostViewModel postViewModel)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var autorId = await _autorRepository.GetByIdentityId(userId);
            if (ModelState.IsValid)
            {
                postViewModel.AutorId = autorId.Id;
                await _postRepository.Add(_mapper.Map<Post>(postViewModel));
                TempData["Success"] = "Post cadastrado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            return View(postViewModel);
        }
        [Route("editar/{id:Guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            var post = _mapper.Map<PostViewModel>(await _postRepository.GetPostAuthorCommentsById(id));
            var userId = Guid.Parse(User.Identity.GetUserId());
            if (post == null || (userId != post.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [HttpPost("editar/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PostViewModel postViewModel)
        {
            if (id != postViewModel.Id)
            {
                return NotFound();
            }
            var post = _mapper.Map<PostViewModel>(await _postRepository.GetPostAuthorCommentsById(id));
            var userId = Guid.Parse(User.Identity.GetUserId());
            if (post == null || (userId != post.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _postRepository.Update(_mapper.Map<Post>(postViewModel));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                TempData["Success"] = "Post editado com sucesso.";

                return RedirectToAction(nameof(Index));
            }
            return View(postViewModel);
        }

        [Route("excluir/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = _mapper.Map<PostViewModel>(await _postRepository.GetPostAuthorCommentsById(id));
            var userId = Guid.Parse(User.Identity.GetUserId());
            if (post == null || (userId != post.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost("excluir/{id:Guid}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var post = _mapper.Map<PostViewModel>(await _postRepository.GetPostAuthorCommentsById(id));
            var userId = Guid.Parse(User.Identity.GetUserId());
            if (post == null || (userId != post.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            if (post == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Aluno'  is null.");
            }
            if (post != null)
            {
                await _postRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
