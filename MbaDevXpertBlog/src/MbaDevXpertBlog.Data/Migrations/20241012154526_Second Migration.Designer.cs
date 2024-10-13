﻿// <auto-generated />
using System;
using MbaDevXpertBlog.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MbaDevXpertBlog.Data.Migrations
{
    [DbContext(typeof(MbaDevXpertBlogDbContext))]
    [Migration("20241012154526_Second Migration")]
    partial class SecondMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MbaDevXpertBlog.Data.Models.Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("IdentityUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Autores", (string)null);
                });

            modelBuilder.Entity("MbaDevXpertBlog.Data.Models.Comentario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.HasIndex("PostId");

                    b.ToTable("Comentarios", (string)null);
                });

            modelBuilder.Entity("MbaDevXpertBlog.Data.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasColumnType("varchar(2000)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("MbaDevXpertBlog.Data.Models.Comentario", b =>
                {
                    b.HasOne("MbaDevXpertBlog.Data.Models.Autor", "Autor")
                        .WithMany("Comentarios")
                        .HasForeignKey("AutorId")
                        .IsRequired();

                    b.HasOne("MbaDevXpertBlog.Data.Models.Post", null)
                        .WithMany("Comentarios")
                        .HasForeignKey("PostId")
                        .IsRequired();

                    b.Navigation("Autor");
                });

            modelBuilder.Entity("MbaDevXpertBlog.Data.Models.Post", b =>
                {
                    b.HasOne("MbaDevXpertBlog.Data.Models.Autor", "Autor")
                        .WithMany("Posts")
                        .HasForeignKey("AutorId")
                        .IsRequired();

                    b.Navigation("Autor");
                });

            modelBuilder.Entity("MbaDevXpertBlog.Data.Models.Autor", b =>
                {
                    b.Navigation("Comentarios");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("MbaDevXpertBlog.Data.Models.Post", b =>
                {
                    b.Navigation("Comentarios");
                });
#pragma warning restore 612, 618
        }
    }
}