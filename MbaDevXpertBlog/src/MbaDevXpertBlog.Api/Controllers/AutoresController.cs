using Microsoft.AspNetCore.Mvc;
using MbaDevXpertBlog.Api.ViewModels;
using MbaDevXpertBlog.Data.Models;
using MbaDevXpertBlog.Data.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MbaDevXpertBlog.Data.Context;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MbaDevXpertBlog.Api.Controllers
{
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly IAutorRepository _autorRepository;
        private readonly IMapper _mapper;
        private readonly MbaDevXpertBlogDbContext _context;

        public AutoresController(IAutorRepository autorRepository,
                                 IMapper mapper,
                                 MbaDevXpertBlogDbContext context)
        {
            _autorRepository = autorRepository;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/autores/listall
        [HttpGet("listall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AutorViewModel>>> GetAllAutores()
        {
            return Ok(_mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.GetAll()));
        }

        // GET: api/autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorViewModel>>> GetAutores()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var autores = await _autorRepository.GetAllWhereUserId(Guid.Parse(userId));
            return Ok(_mapper.Map<IEnumerable<AutorViewModel>>(autores));
        }

        // GET: api/autores/{id}
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<AutorViewModel>> GetAutor(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var autor = _mapper.Map<AutorViewModel>(await _autorRepository.GetById(id));
            if (autor == null || (Guid.Parse(userId) != autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            return Ok(autor);
        }

        // POST: api/autores
        [HttpPost]
        public async Task<ActionResult> CreateAutor(AutorViewModel autorViewModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            autorViewModel.Id = Guid.Parse(userId);
            var autorExists = await _autorRepository.Search(u => u.Id == Guid.Parse(userId));
            if (autorExists.Any())
            {
                return BadRequest("Seu usuário já possui um autor cadastrado.");
            }

            if (ModelState.IsValid)
            {
                await _autorRepository.Add(_mapper.Map<Autor>(autorViewModel));
                return Ok("Autor cadastrado com sucesso.");
            }

            return BadRequest(ModelState);
        }

        // PUT: api/autores/{id}
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> EditAutor(Guid id, AutorViewModel autorViewModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var autor = _mapper.Map<AutorViewModel>(await _autorRepository.GetById(id));
            if (autor == null || (Guid.Parse(userId) != autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            if (id != autorViewModel.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _autorRepository.Update(_mapper.Map<Autor>(autorViewModel));
                    return Ok("Autor editado com sucesso.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/autores/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAutor(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var autor = _mapper.Map<AutorViewModel>(await _autorRepository.GetById(id));
            if (autor == null || (Guid.Parse(userId) != autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            if (autor != null)
            {
                await _autorRepository.Delete(id);
                return Ok("Autor excluído com sucesso.");
            }

            return NotFound();
        }
    }
}
