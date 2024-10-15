using MbaDevXpertBlog.Data.Context;
using MbaDevXpertBlog.Data.Data;
using MbaDevXpertBlog.Data.Extensions;
using MbaDevXpertBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace MbaDevXpertBlog.Mvc.Configuration
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

            var context = scope.ServiceProvider.GetRequiredService<MbaDevXpertBlogDbContext>();
            var contextId = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker")) 
            {
                await context.Database.MigrateAsync();
                await contextId.Database.MigrateAsync();

                await EnsureSeedProducts(context, contextId);
            }

        }
        private static async Task EnsureSeedProducts(MbaDevXpertBlogDbContext context, ApplicationDbContext contextId)
        {
            if (contextId.Users.Any())
                return;
            var userId = Guid.NewGuid();
            await contextId.Users.AddAsync(new ApplicationUser
            {
                Id = userId,
                UserName = "teste@teste.com",
                NormalizedUserName = "TESTE@TESTE.COM",
                Email = "teste@teste.com",
                NormalizedEmail = "TESTE@TESTE.COM",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEEdWhqiCwW/jZz0hEM7aNjok7IxniahnxKxxO5zsx2TvWs4ht1FUDnYofR8JKsA5UA==",
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            });

            await contextId.SaveChangesAsync();

            if (context.Autores.Any())
                return;
            await context.Autores.AddAsync(new Autor
            {
                Id = userId,
                Nome = "Eduardo Pires",
                Email = "eduardo@desenvolvedor.io"
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
