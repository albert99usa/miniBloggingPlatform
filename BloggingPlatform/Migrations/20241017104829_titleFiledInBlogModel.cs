using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloggingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class titleFiledInBlogModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Blog");
        }
    }
}
