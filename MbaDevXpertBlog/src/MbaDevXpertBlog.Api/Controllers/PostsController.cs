using AutoMapper;
using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Interfaces;
using MbaDevXpertBlog.Data.Models;
using MbaDevXpertBlog.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MbaDevXpertBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
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

        // GET: api/posts/listall
        [HttpGet("listall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetAllPosts()
        {
            return Ok(_mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.GetAll()));
        }

        // GET: api/posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetPostsByAuthor()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var autor = await _autorRepository.GetByIdentityId(Guid.Parse(userId));
            if (autor == null)
            {
                return BadRequest("É necessário criar um autor antes de ver seus posts.");
            }

            var posts = await _postRepository.GetAllPostsByAuthorId(autor.Id);
            return Ok(_mapper.Map<IEnumerable<PostViewModel>>(posts));
        }

        // GET: api/posts/{id}
        [HttpGet("{id:Guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostDetailsViewModel>> GetPostDetails(Guid id)
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

            return Ok(postDetailsViewModel);
        }

        // POST: api/posts
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreatePost([FromBody] PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var autorId = await _autorRepository.GetByIdentityId(Guid.Parse(userId));
            postViewModel.AutorId = autorId.Id;

            await _postRepository.Add(_mapper.Map<Post>(postViewModel));
            return CreatedAtAction(nameof(GetPostDetails), new { id = postViewModel.Id }, postViewModel);
        }

        // PUT: api/posts/{id}
        [HttpPut("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> EditPost(Guid id, [FromBody] PostViewModel postViewModel)
        {
            if (id != postViewModel.Id)
            {
                return BadRequest("Post ID mismatch.");
            }

            var post = _mapper.Map<PostViewModel>(await _postRepository.GetPostAuthorCommentsById(id));
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (post == null || (Guid.Parse(userId) != post.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _postRepository.Update(_mapper.Map<Post>(postViewModel));
                    return Ok("Post editado com sucesso.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar o post.");
                }
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/posts/{id}
        [HttpDelete("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var post = _mapper.Map<PostViewModel>(await _postRepository.GetPostAuthorCommentsById(id));
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (post == null || (Guid.Parse(userId) != post.Autor.IdentityUserId && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            await _postRepository.Delete(id);
            return Ok("Post excluído com sucesso.");
        }
    }
}
