using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloggingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_ParentID",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_CommentID",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Entry_EntryID",
                table: "Comment");

            migrationBuilder.DropTable(
                name: "CategoryEntry");

            migrationBuilder.DropIndex(
                name: "IX_Comment_CommentID",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_EntryID",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CommentID",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "EntryID",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Comment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CommentStatus",
                table: "Comment",
                newName: "IsApproved");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Category",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ParentID",
                table: "Category",
                newName: "EntryID");

            migrationBuilder.RenameIndex(
                name: "IX_Category_ParentID",
                table: "Category",
                newName: "IX_Category_EntryID");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Blog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Blog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_BlogId",
                table: "Comment",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_CategoryId",
                table: "Blog",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Category_CategoryId",
                table: "Blog",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Entry_EntryID",
                table: "Category",
                column: "EntryID",
                principalTable: "Entry",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Blog_BlogId",
                table: "Comment",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Category_CategoryId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Entry_EntryID",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Blog_BlogId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_BlogId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Blog_CategoryId",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Blog");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comment",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "IsApproved",
                table: "Comment",
                newName: "CommentStatus");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Category",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "EntryID",
                table: "Category",
                newName: "ParentID");

            migrationBuilder.RenameIndex(
                name: "IX_Category_EntryID",
                table: "Category",
                newName: "IX_Category_ParentID");

            migrationBuilder.AddColumn<int>(
                name: "CommentID",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Comment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntryID",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Category",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CategoryEntry",
                columns: table => new
                {
                    CategoriesID = table.Column<int>(type: "int", nullable: false),
                    EntriesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntry", x => new { x.CategoriesID, x.EntriesID });
                    table.ForeignKey(
                        name: "FK_CategoryEntry_Category_CategoriesID",
                        column: x => x.CategoriesID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryEntry_Entry_EntriesID",
                        column: x => x.EntriesID,
                        principalTable: "Entry",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CommentID",
                table: "Comment",
                column: "CommentID");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_EntryID",
                table: "Comment",
                column: "EntryID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEntry_EntriesID",
                table: "CategoryEntry",
                column: "EntriesID");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_ParentID",
                table: "Category",
                column: "ParentID",
                principalTable: "Category",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_CommentID",
                table: "Comment",
                column: "CommentID",
                principalTable: "Comment",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Entry_EntryID",
                table: "Comment",
                column: "EntryID",
                principalTable: "Entry",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
