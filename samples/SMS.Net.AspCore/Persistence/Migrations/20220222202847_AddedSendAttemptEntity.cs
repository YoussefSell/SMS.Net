using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Net.AspCore.Migrations
{
    public partial class AddedSendAttemptEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RavenSmsMessageSendAttempt",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Errors = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageId = table.Column<string>(type: "varchar(17)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RavenSmsMessageSendAttempt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RavenSmsMessageSendAttempt_RavenSmsMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "RavenSmsMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RavenSmsMessageSendAttempt_MessageId",
                table: "RavenSmsMessageSendAttempt",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RavenSmsMessageSendAttempt");
        }
    }
}
