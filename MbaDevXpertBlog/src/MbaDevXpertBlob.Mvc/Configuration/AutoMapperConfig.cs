using AutoMapper;
using MbaDevXpertBlog.Data.Models;
using MbaDevXpertBlog.Mvc.ViewModels;

namespace MbaDevXpertBlog.Mvc.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Post, PostViewModel>().ReverseMap();
            CreateMap<Autor, AutorViewModel>().ReverseMap();
            CreateMap<Comentario, ComentarioViewModel>().ReverseMap();
        }
    }
}

