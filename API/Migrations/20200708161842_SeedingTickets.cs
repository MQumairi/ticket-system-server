using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class SeedingTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "commentIds",
                table: "tickets");

            migrationBuilder.InsertData(
                table: "tickets",
                columns: new[] { "Id", "authorId", "date", "description", "product", "status", "title" },
                values: new object[,]
                {
                    { 1, 1, "2020-01-04", "The app installation crashes after updating to version 10.15.5.", "Product 1", "Urgent", "Crashes after update" },
                    { 2, 2, "2020-01-04", "Displays a 502 error w", "Product 1", "Low", "Not connecting to database" },
                    { 3, 2, "2020-01-04", "The app installation crashes after updating to version 10.15.5.", "Product 2", "Low", "Crashes after update" },
                    { 4, 1, "2020-01-04", "The app installation crashes after updating to version 10.15.5.", "Product 4", "Done", "Crashes after update" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tickets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tickets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tickets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tickets",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
