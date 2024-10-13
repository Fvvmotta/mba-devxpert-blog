using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MbaDevXpertBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_PostId",
                table: "Comentarios",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Posts_PostId",
                table: "Comentarios",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Posts_PostId",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_PostId",
                table: "Comentarios");
        }
    }
}
