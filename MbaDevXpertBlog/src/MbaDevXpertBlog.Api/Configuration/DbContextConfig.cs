using MbaDevXpertBlog.Data.Data;
using MbaDevXpertBlog.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MbaDevXpertBlog.Api.Configuration
{
    public static class DbContextConfig
    {
        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
           

            //Blog DBContext Config
            builder.Services.AddDbContext<MbaDevXpertBlogDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            return builder;
        }
    }
}
