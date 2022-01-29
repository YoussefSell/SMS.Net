using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Net.AspCore.Migrations
{
    public partial class AddedClientPhoneNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumbers",
                table: "RavenSmsClients");

            migrationBuilder.CreateTable(
                name: "RavenSmsClientPhoneNumber",
                columns: table => new
                {
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RavenSmsClientPhoneNumber", x => x.PhoneNumber);
                    table.ForeignKey(
                        name: "FK_RavenSmsClientPhoneNumber_RavenSmsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "RavenSmsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RavenSmsClientPhoneNumber_ClientId",
                table: "RavenSmsClientPhoneNumber",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RavenSmsClientPhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumbers",
                table: "RavenSmsClients",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
