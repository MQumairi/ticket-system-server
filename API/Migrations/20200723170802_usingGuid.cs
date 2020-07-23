using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class usingGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    ticketNumb = table.Column<Guid>(nullable: false),
                    authorId = table.Column<int>(nullable: false),
                    status = table.Column<string>(nullable: false),
                    product = table.Column<string>(nullable: false),
                    title = table.Column<string>(nullable: false),
                    date = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    testfield = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.ticketNumb);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tickets");
        }
    }
}
