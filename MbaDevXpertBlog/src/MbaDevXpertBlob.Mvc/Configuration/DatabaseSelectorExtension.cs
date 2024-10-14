using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace MbaDevXpertBlog.Mvc.Configuration
{
    public static class DatabaseSelectorExtension
    {
        public static void AddDatabaseSelector(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite")));

                //Blog DBContext Config
                builder.Services.AddDbContext<MbaDevXpertBlogDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite")));
            }
            else
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                //Blog DBContext Config
                builder.Services.AddDbContext<MbaDevXpertBlogDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            }
        }
    }
}
