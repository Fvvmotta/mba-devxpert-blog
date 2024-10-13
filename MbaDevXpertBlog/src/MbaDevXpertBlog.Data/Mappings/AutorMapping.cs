using MbaDevXpertBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbaDevXpertBlog.Data.Mappings
{
    public class AutorMapping : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Nome)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.HasMany(a => a.Posts)
                .WithOne(p => p.Autor)
                .HasForeignKey(p => p.AutorId);

            builder.HasMany(a => a.Comentarios)
               .WithOne(p => p.Autor)
               .HasForeignKey(c => c.AutorId);

            builder.ToTable("Autores");
        }
    }
}
