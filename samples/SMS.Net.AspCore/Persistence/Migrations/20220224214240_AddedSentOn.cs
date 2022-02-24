﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Net.AspCore.Migrations
{
    public partial class AddedSentOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SentOn",
                table: "RavenSmsMessages",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentOn",
                table: "RavenSmsMessages");
        }
    }
}
