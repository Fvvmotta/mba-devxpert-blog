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
    [Authorize]
    [Route("api/comentarios")]
    [ApiController]
    public class ComentariosController : ControllerBase
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

        // GET: api/comentarios/listall
        [HttpGet("listall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ComentarioViewModel>>> GetAllComentarios()
        {
            var comentarios = await _comentarioRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<ComentarioViewModel>>(comentarios));
        }

        // GET: api/comentarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComentarioViewModel>>> GetComentariosByAuthor()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var autor = await _autorRepository.GetById(Guid.Parse(userId));
            if (autor == null)
            {
                return BadRequest("É necessário criar um autor antes de ver seus comentários.");
            }

            var comentarios = await _comentarioRepository.GetCommentByAuthorId(autor.Id);
            return Ok(_mapper.Map<IEnumerable<ComentarioViewModel>>(comentarios));
        }

        // GET: api/comentarios/{id}
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ComentarioViewModel>> GetComentarioById(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.GetCommentAuthorById(id));
            if (comentario == null || (Guid.Parse(userId) != comentario.Autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            return Ok(comentario);
        }

        // POST: api/comentarios/novo
        [HttpPost("novo")]
        public async Task<ActionResult<ComentarioViewModel>> CreateComentario(ComentarioViewModel comentarioViewModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var autorId = await _autorRepository.GetById(Guid.Parse(userId));

            if (autorId == null)
            {
                return BadRequest("Autor not found.");
            }

            comentarioViewModel.AutorId = autorId.Id;

            if (ModelState.IsValid)
            {
                var comentario = _mapper.Map<Comentario>(comentarioViewModel);
                await _comentarioRepository.Add(comentario);

                // Return 201 Created with the location of the new resource
                return CreatedAtAction(nameof(GetComentarioById), new { id = comentario.Id }, comentarioViewModel);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/comentarios/editar/{id}
        [HttpPut("editar/{id:Guid}")]
        public async Task<ActionResult> EditComentario(Guid id, ComentarioViewModel comentarioViewModel)
        {
            if (id != comentarioViewModel.Id)
            {
                return BadRequest();
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.GetCommentAuthorById(id));

            if (comentario == null || (Guid.Parse(userId) != comentario.Autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var comentarioEntity = _mapper.Map<Comentario>(comentarioViewModel);
                    await _comentarioRepository.Update(comentarioEntity);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/comentarios/excluir/{id}
        [HttpDelete("excluir/{id:Guid}")]
        public async Task<ActionResult> DeleteComentario(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.GetCommentAuthorById(id));

            if (comentario == null || (Guid.Parse(userId) != comentario.Autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            await _comentarioRepository.Delete(id);
            return NoContent();
        }
    }
}
