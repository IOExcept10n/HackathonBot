using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackathonBot.Migrations
{
    /// <inheritdoc />
    public partial class AddedEventAuditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventEntry_Event_EventId",
                table: "EventEntry");

            migrationBuilder.DropIndex(
                name: "IX_EventEntry_EventId",
                table: "EventEntry");

            migrationBuilder.DropIndex(
                name: "IX_EventEntry_KmmTeamId_EventId_QuestId",
                table: "EventEntry");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "EventEntry",
                newName: "Place");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "KmmTeam",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "TargetTeamId",
                table: "AbilityUse",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventAuditEntry",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InitiatorId = table.Column<long>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false),
                    LoggedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAuditEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventAuditEntry_Users_InitiatorId",
                        column: x => x.InitiatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventEntry_KmmTeamId_QuestId",
                table: "EventEntry",
                columns: new[] { "KmmTeamId", "QuestId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbilityUse_TargetTeamId",
                table: "AbilityUse",
                column: "TargetTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_EventAuditEntry_EventType_LoggedAt",
                table: "EventAuditEntry",
                columns: new[] { "EventType", "LoggedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EventAuditEntry_InitiatorId",
                table: "EventAuditEntry",
                column: "InitiatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbilityUse_KmmTeam_TargetTeamId",
                table: "AbilityUse",
                column: "TargetTeamId",
                principalTable: "KmmTeam",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbilityUse_KmmTeam_TargetTeamId",
                table: "AbilityUse");

            migrationBuilder.DropTable(
                name: "EventAuditEntry");

            migrationBuilder.DropIndex(
                name: "IX_EventEntry_KmmTeamId_QuestId",
                table: "EventEntry");

            migrationBuilder.DropIndex(
                name: "IX_AbilityUse_TargetTeamId",
                table: "AbilityUse");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "KmmTeam");

            migrationBuilder.DropColumn(
                name: "TargetTeamId",
                table: "AbilityUse");

            migrationBuilder.RenameColumn(
                name: "Place",
                table: "EventEntry",
                newName: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventEntry_EventId",
                table: "EventEntry",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventEntry_KmmTeamId_EventId_QuestId",
                table: "EventEntry",
                columns: new[] { "KmmTeamId", "EventId", "QuestId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventEntry_Event_EventId",
                table: "EventEntry",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
