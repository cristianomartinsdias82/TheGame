using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheGame.Infrastructure.Data.Ef.Migrations
{
    public partial class PlayerScoreLastUpdateOnDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ScoreLastUpdateOn",
                table: "Players",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Player_ScoreLastUpdateOn",
                table: "Players",
                column: "ScoreLastUpdateOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Player_ScoreLastUpdateOn",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ScoreLastUpdateOn",
                table: "Players");
        }
    }
}
