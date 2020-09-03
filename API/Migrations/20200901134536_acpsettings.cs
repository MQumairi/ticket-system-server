using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.Migrations
{
    public partial class acpsettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "can_manage",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "can_moderate",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "acp_settings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    founder_id = table.Column<string>(nullable: false),
                    registration_locked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acp_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_acp_settings_AspNetUsers_founder_id",
                        column: x => x.founder_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_acp_settings_founder_id",
                table: "acp_settings",
                column: "founder_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "acp_settings");

            migrationBuilder.DropColumn(
                name: "can_manage",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "can_moderate",
                table: "AspNetRoles");
        }
    }
}
