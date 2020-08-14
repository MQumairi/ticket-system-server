using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ticketArchivesAndDevs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "developer_id",
                table: "posts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_posts_developer_id",
                table: "posts",
                column: "developer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_posts_AspNetUsers_developer_id",
                table: "posts",
                column: "developer_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_AspNetUsers_developer_id",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_developer_id",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "developer_id",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "posts");
        }
    }
}
