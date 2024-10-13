using AutoMapper;
using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Interfaces;
using MbaDevXpertBlog.Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MbaDevXpertBlob.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IAutorRepository _autorRepository;
        private readonly IMapper _mapper;
        private readonly MbaDevXpertBlogDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, 
                            IPostRepository postRepository,
                               IAutorRepository autorRepository,
                               IMapper mapper,
                               MbaDevXpertBlogDbContext context)
        { 
            _logger = logger;
            _postRepository = postRepository;
            _autorRepository = autorRepository;
            _mapper = mapper;
            _context = context;
        }
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.GetAllPostAuthor()));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
