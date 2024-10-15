using Microsoft.AspNetCore.Mvc;
using MbaDevXpertBlog.Mvc.ViewModels;
using MbaDevXpertBlog.Data.Models;
using MbaDevXpertBlog.Data.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MbaDevXpertBlog.Data.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using MbaDevXpertBlog.Data.Repository;

namespace MbaDevXpertBlog.Mvc.Controllers
{
    [Route("autores")]
    public class AutoresController : Controller
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
        [Authorize(Roles = "Admin")]
        [Route("listall")]
        public async Task<IActionResult> IndexAll()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            return View("index", _mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.GetAll()));
        }
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            return View(_mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.GetAllWhereUserId(userId)));
        }
        [Route("{id:Guid}/detalhes")]
        public async Task<ActionResult> Details(Guid id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var autor = _mapper.Map<AutorViewModel>(await _autorRepository.GetById(id));
            if (autor == null || (userId != autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            return View(autor);
        }
        [Route("novo")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost("novo")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AutorViewModel autorViewModel)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            autorViewModel.Id = userId;
            var autorExiste = await _autorRepository.Search(u => u.Id == userId);
            if (autorExiste.Count() > 0)
            {
                TempData["Error"] = "Seu usuário já possui um autor cadastrado.";
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                await _autorRepository.Add(_mapper.Map<Autor>(autorViewModel));
                TempData["Success"] = "Autor cadastrado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            return View(autorViewModel);
        }
        [Route("editar/{id:int}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var autor = _mapper.Map<AutorViewModel>(await _autorRepository.GetById(id));

            if (autor == null || (userId != autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            return View(autor);
        }
        [HttpPost("editar/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AutorViewModel autorViewModel)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var autor = _mapper.Map<AutorViewModel>(await _autorRepository.GetById(id));
            if (autor == null || (userId != autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            if (id != autorViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _autorRepository.Update(_mapper.Map<Autor>(autorViewModel));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                TempData["Success"] = "Autor editado com sucesso.";

                return RedirectToAction(nameof(Index));
            }
            return View(autorViewModel);
        }

        [Route("excluir/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var autor = _mapper.Map<AutorViewModel>(await _autorRepository.GetById(id));
            if (autor == null || (userId != autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            return View(autor);
        }

        [HttpPost("excluir/{id:Guid}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var autor = _mapper.Map<AutorViewModel>(await _autorRepository.GetById(id));
            if (autor == null || (userId != autor.Id && !User.IsInRole("Admin")))
            {
                return NotFound();
            }
            if (autor != null)
            {
                await _autorRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}