using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class handlingPhotos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "attachment_url",
                table: "posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "surname",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "photos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    url = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_posts_attachment_url",
                table: "posts",
                column: "attachment_url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_avatar_url",
                table: "AspNetUsers",
                column: "avatar_url",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_photos_avatar_url",
                table: "AspNetUsers",
                column: "avatar_url",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_photos_attachment_url",
                table: "posts",
                column: "attachment_url",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_photos_avatar_url",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_photos_attachment_url",
                table: "posts");

            migrationBuilder.DropTable(
                name: "photos");

            migrationBuilder.DropIndex(
                name: "IX_posts_attachment_url",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_avatar_url",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "attachment_url",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "surname",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
