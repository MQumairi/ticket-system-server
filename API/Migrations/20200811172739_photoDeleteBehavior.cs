using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class photoDeleteBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_photos_avatar_id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_photos_attachment_id",
                table: "posts");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_photos_avatar_id",
                table: "AspNetUsers",
                column: "avatar_id",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_photos_attachment_id",
                table: "posts",
                column: "attachment_id",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_photos_avatar_id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_photos_attachment_id",
                table: "posts");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_photos_avatar_id",
                table: "AspNetUsers",
                column: "avatar_id",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_photos_attachment_id",
                table: "posts",
                column: "attachment_id",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
