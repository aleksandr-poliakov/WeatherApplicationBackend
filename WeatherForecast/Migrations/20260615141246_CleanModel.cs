using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherForecast.Migrations
{
    /// <inheritdoc />
    public partial class CleanModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerMatchDayAbsences");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerMatchDayAbsences",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchDayId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMatchDayAbsences", x => new { x.PlayerId, x.MatchDayId });
                    table.ForeignKey(
                        name: "FK_PlayerMatchDayAbsences_MatchDays_MatchDayId",
                        column: x => x.MatchDayId,
                        principalTable: "MatchDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMatchDayAbsences_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchDayAbsences_MatchDayId",
                table: "PlayerMatchDayAbsences",
                column: "MatchDayId");
        }
    }
}
