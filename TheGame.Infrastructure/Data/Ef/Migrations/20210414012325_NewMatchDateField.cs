using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheGame.Infrastructure.Data.Ef.Migrations
{
    public partial class NewMatchDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "MatchDate",
                table: "GameMatchesPlayers",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchDate",
                table: "GameMatchesPlayers");
        }
    }
}
