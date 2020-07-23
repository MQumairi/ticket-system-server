using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class removeTestField2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "testfield",
                table: "tickets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "testfield",
                table: "tickets",
                type: "text",
                nullable: true);
        }
    }
}
