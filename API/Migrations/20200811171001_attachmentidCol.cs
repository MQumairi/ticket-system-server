using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class attachmentidCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_photos_attachment_url",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_attachment_url",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "attachment_url",
                table: "posts");

            migrationBuilder.AddColumn<string>(
                name: "attachment_id",
                table: "posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_posts_attachment_id",
                table: "posts",
                column: "attachment_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_photos_attachment_id",
                table: "posts",
                column: "attachment_id",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_photos_attachment_id",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_attachment_id",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "attachment_id",
                table: "posts");

            migrationBuilder.AddColumn<string>(
                name: "attachment_url",
                table: "posts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_posts_attachment_url",
                table: "posts",
                column: "attachment_url",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_photos_attachment_url",
                table: "posts",
                column: "attachment_url",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
