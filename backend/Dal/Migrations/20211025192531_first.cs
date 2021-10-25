using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dal.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TakeoffDestination = table.Column<string>(type: "TEXT", nullable: true),
                    LandingDestination = table.Column<string>(type: "TEXT", nullable: true),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    WaitingTime = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirportStationsState",
                columns: table => new
                {
                    StationId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlightId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirportStationsState", x => x.StationId);
                    table.ForeignKey(
                        name: "FK_AirportStationsState_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AirportStationsState_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightStationLogger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ToStationId = table.Column<int>(type: "INTEGER", nullable: true),
                    FromStationId = table.Column<int>(type: "INTEGER", nullable: true),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    EntranceStaionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightStationLogger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightStationLogger_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightStationLogger_Stations_FromStationId",
                        column: x => x.FromStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightStationLogger_Stations_ToStationId",
                        column: x => x.ToStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StationsRelation",
                columns: table => new
                {
                    ToStationId = table.Column<int>(type: "INTEGER", nullable: false),
                    FromStationId = table.Column<int>(type: "INTEGER", nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationsRelation", x => new { x.FromStationId, x.ToStationId, x.Direction });
                    table.ForeignKey(
                        name: "FK_StationsRelation_Stations_FromStationId",
                        column: x => x.FromStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StationsRelation_Stations_ToStationId",
                        column: x => x.ToStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirportStationsState_FlightId",
                table: "AirportStationsState",
                column: "FlightId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightStationLogger_FlightId",
                table: "FlightStationLogger",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightStationLogger_FromStationId",
                table: "FlightStationLogger",
                column: "FromStationId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightStationLogger_ToStationId",
                table: "FlightStationLogger",
                column: "ToStationId");

            migrationBuilder.CreateIndex(
                name: "IX_StationsRelation_ToStationId",
                table: "StationsRelation",
                column: "ToStationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirportStationsState");

            migrationBuilder.DropTable(
                name: "FlightStationLogger");

            migrationBuilder.DropTable(
                name: "StationsRelation");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Stations");
        }
    }
}
