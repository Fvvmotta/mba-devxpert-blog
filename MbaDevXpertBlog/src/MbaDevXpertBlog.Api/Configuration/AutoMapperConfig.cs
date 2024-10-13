using AutoMapper;
using MbaDevXpertBlog.Data.Models;
using MbaDevXpertBlog.Api.ViewModels;

namespace MbaDevXpertBlog.Api.Configuration
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

