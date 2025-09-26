using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackathonBot.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_TelegramId",
                table: "Users",
                column: "TelegramId");

            migrationBuilder.CreateTable(
                name: "BotUserRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TelegramId = table.Column<long>(type: "INTEGER", nullable: true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Note = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotUserRole_Users_TelegramId",
                        column: x => x.TelegramId,
                        principalTable: "Users",
                        principalColumn: "TelegramId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Case = table.Column<int>(type: "INTEGER", nullable: false),
                    KmmId = table.Column<long>(type: "INTEGER", nullable: false),
                    SubmissionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nickname = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TelegramId = table.Column<long>(type: "INTEGER", nullable: true),
                    FsmUserId = table.Column<long>(type: "INTEGER", nullable: true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsLeader = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participant_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Participant_Users_FsmUserId",
                        column: x => x.FsmUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Participant_Users_TelegramId",
                        column: x => x.TelegramId,
                        principalTable: "Users",
                        principalColumn: "TelegramId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Submission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Case = table.Column<int>(type: "INTEGER", nullable: false),
                    RepoUrl = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    PresentationFileUrl = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    PresentationLink = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubmittedById = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submission_Participant_SubmittedById",
                        column: x => x.SubmittedById,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submission_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotUserRole_TelegramId",
                table: "BotUserRole",
                column: "TelegramId");

            migrationBuilder.CreateIndex(
                name: "IX_BotUserRole_Username",
                table: "BotUserRole",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participant_FsmUserId",
                table: "Participant",
                column: "FsmUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_TeamId",
                table: "Participant",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_TelegramId",
                table: "Participant",
                column: "TelegramId");

            migrationBuilder.CreateIndex(
                name: "IX_Submission_SubmittedById",
                table: "Submission",
                column: "SubmittedById");

            migrationBuilder.CreateIndex(
                name: "IX_Submission_TeamId",
                table: "Submission",
                column: "TeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_Name",
                table: "Team",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotUserRole");

            migrationBuilder.DropTable(
                name: "Submission");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_TelegramId",
                table: "Users");
        }
    }
}
