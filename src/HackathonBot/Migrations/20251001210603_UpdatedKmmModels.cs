using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackathonBot.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedKmmModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HackathonTeamId",
                table: "KmmTeam",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_KmmTeam_HackathonTeamId",
                table: "KmmTeam",
                column: "HackathonTeamId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KmmTeam_Team_HackathonTeamId",
                table: "KmmTeam",
                column: "HackathonTeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KmmTeam_Team_HackathonTeamId",
                table: "KmmTeam");

            migrationBuilder.DropIndex(
                name: "IX_KmmTeam_HackathonTeamId",
                table: "KmmTeam");

            migrationBuilder.DropColumn(
                name: "HackathonTeamId",
                table: "KmmTeam");
        }
    }
}
