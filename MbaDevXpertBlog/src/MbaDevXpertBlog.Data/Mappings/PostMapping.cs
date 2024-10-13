using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MbaDevXpertBlog.Data.Models;

namespace MbaDevXpertBlog.Data.Mappings
{
    public class PostMapping : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Conteudo)
                .IsRequired()
                .HasColumnType("varchar(2000)");
            builder.ToTable("Posts");

            builder.HasOne(c => c.Autor)
                .WithMany(e => e.Posts);
        }
    }
}
