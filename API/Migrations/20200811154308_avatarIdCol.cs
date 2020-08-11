using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class avatarIdCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_photos_avatar_url",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_avatar_url",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "surname",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatar_id",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_avatar_id",
                table: "AspNetUsers",
                column: "avatar_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_photos_avatar_id",
                table: "AspNetUsers",
                column: "avatar_id",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_photos_avatar_id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_avatar_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "avatar_id",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "surname",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

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
        }
    }
}
