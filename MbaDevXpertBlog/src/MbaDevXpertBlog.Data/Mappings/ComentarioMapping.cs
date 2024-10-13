using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MbaDevXpertBlog.Data.Models;

namespace MbaDevXpertBlog.Data.Mappings
{
    public class ComentarioMapping : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Conteudo)
                .IsRequired()
                .HasColumnType("varchar(250)");
            builder.ToTable("Comentarios");
            builder.HasOne(c => c.Autor)
                .WithMany(e => e.Comentarios);
        }
    }
}
