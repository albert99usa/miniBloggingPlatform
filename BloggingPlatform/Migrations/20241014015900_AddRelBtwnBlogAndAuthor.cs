using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloggingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddRelBtwnBlogAndAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Author_AuthorID",
                table: "Blog");

            migrationBuilder.RenameColumn(
                name: "AuthorID",
                table: "Blog",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Blog_AuthorID",
                table: "Blog",
                newName: "IX_Blog_AuthorId");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Blog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Author_AuthorId",
                table: "Blog",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Author_AuthorId",
                table: "Blog");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Blog",
                newName: "AuthorID");

            migrationBuilder.RenameIndex(
                name: "IX_Blog_AuthorId",
                table: "Blog",
                newName: "IX_Blog_AuthorID");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorID",
                table: "Blog",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Author_AuthorID",
                table: "Blog",
                column: "AuthorID",
                principalTable: "Author",
                principalColumn: "ID");
        }
    }
}
