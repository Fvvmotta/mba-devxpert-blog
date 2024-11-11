using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Data;
using MbaDevXpertBlog.Data.Extensions;
using MbaDevXpertBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace MbaDevXpertBlog.Api.Configuration
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope =serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var context = scope.ServiceProvider.GetRequiredService<MbaDevXpertBlogDbContext>();
            var contextId = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
          

            if (env.IsDevelopment() || env.IsEnvironment("Docker")) 
            {
                await context.Database.MigrateAsync();
                await contextId.Database.MigrateAsync();

                await EnsureSeedProducts(context, contextId, roleManager, userManager);
            }

        }
        private static async Task EnsureSeedProducts(MbaDevXpertBlogDbContext context, ApplicationDbContext contextId, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (contextId.Users.Any())
                return;
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole("Admin"));
            }
            var adminId = Guid.NewGuid();

            if (!contextId.Users.Any(u => u.UserName == "admin@teste.com"))
            {
                var adminUser = new ApplicationUser
                {
                    Id = adminId,
                    UserName = "admin@teste.com",
                    NormalizedUserName = "ADMIN@TESTE.COM",
                    Email = "admin@teste.com",
                    NormalizedEmail = "ADMIN@TESTE.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                adminUser.PasswordHash = userManager.PasswordHasher.HashPassword(adminUser, "Admin@123");

                await userManager.CreateAsync(adminUser);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            await context.Autores.AddAsync(new Autor
            {
                Id = adminId,
                Nome = "Admin User",
                Email = "admin@admin.com"
            });

            var userId = Guid.NewGuid();
            if (!contextId.Users.Any(u => u.UserName == "teste@teste.com"))
            {
                var normalUser = new ApplicationUser
                {
                    Id = userId,
                    UserName = "teste@teste.com",
                    NormalizedUserName = "TESTE@TESTE.COM",
                    Email = "teste@teste.com",
                    NormalizedEmail = "TESTE@TESTE.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                normalUser.PasswordHash = userManager.PasswordHasher.HashPassword(normalUser, "Teste@123");

                await userManager.CreateAsync(normalUser);
            }

            if (context.Autores.Any())
                return;
            await context.Autores.AddAsync(new Autor
            {
                Id = userId,
                Nome = "Teste User",
                Email = "teste@teste.com"
            }) ;
            await context.SaveChangesAsync();

            var postIdOne = Guid.NewGuid();
            await context.Posts.AddAsync(new Post()
            {
                Id = postIdOne,
                AutorId = userId,
                Titulo = "Meu Primeiro Post no Blog",
                Conteudo = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum lobortis dui quis nisi vehicula, id finibus leo hendrerit. Ut nulla metus, iaculis in augue quis, pharetra scelerisque tortor. Sed egestas mollis finibus. Etiam eget felis eget nunc venenatis tincidunt eget vitae sem. Maecenas faucibus nunc eu orci fermentum, quis gravida turpis rhoncus. Nunc magna libero, hendrerit quis lorem blandit, congue auctor nisi. Pellentesque lobortis tincidunt tellus, eget aliquet quam bibendum vel."
            }); 
            await context.SaveChangesAsync();

            var postIdOTwo = Guid.NewGuid();
            await context.Posts.AddAsync(new Post()
            {
                Id = postIdOTwo,
                AutorId = userId,
                Titulo = "Meu Segundo Post no Blog",
                Conteudo = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum lobortis dui quis nisi vehicula, id finibus leo hendrerit. Ut nulla metus, iaculis in augue quis, pharetra scelerisque tortor. Sed egestas mollis finibus. Etiam eget felis eget nunc venenatis tincidunt eget vitae sem. Maecenas faucibus nunc eu orci fermentum, quis gravida turpis rhoncus. Nunc magna libero, hendrerit quis lorem blandit, congue auctor nisi. Pellentesque lobortis tincidunt tellus, eget aliquet quam bibendum vel."
            });

            await context.SaveChangesAsync();

            var comentarioIdOne = Guid.NewGuid();
            await context.Comentarios.AddAsync(new Comentario()
            {
                Id = comentarioIdOne,
                PostId = postIdOne,
                AutorId = userId,
                Conteudo = "Esse post está bem bacana."
            });
            await context.SaveChangesAsync();

            var comentarioIdTwo = Guid.NewGuid();
            await context.Comentarios.AddAsync(new Comentario()
            {
                Id = comentarioIdTwo,
                PostId = postIdOne,
                AutorId = userId,
                Conteudo = "Sensacional!"
            });
            await context.SaveChangesAsync();
        }
    }
}
