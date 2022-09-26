using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreeDOW.API.DataAccess.Migrations.WorkTimeManageDb
{
    public partial class wt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FreeDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DTStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DTEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Confirmed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OverWorks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DTStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DTEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OverheadAddTime = table.Column<int>(type: "integer", nullable: false),
                    EquipmentDetail = table.Column<string>(type: "text", nullable: false),
                    Desription = table.Column<string>(type: "text", nullable: false),
                    Confirmed = table.Column<int>(type: "integer", nullable: false),
                    Reminder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverWorks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vacations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DTStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DTEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Confirmed = table.Column<int>(type: "integer", nullable: false),
                    VacationType = table.Column<Guid>(type: "uuid", nullable: false),
                    FactDTStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FactDTEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OWFDS",
                columns: table => new
                {
                    OverWorkId = table.Column<Guid>(type: "uuid", nullable: false),
                    FreeDayId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeTaken = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OWFDS", x => new { x.FreeDayId, x.OverWorkId });
                    table.ForeignKey(
                        name: "FK_OWFDS_FreeDays_FreeDayId",
                        column: x => x.FreeDayId,
                        principalTable: "FreeDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OWFDS_OverWorks_OverWorkId",
                        column: x => x.OverWorkId,
                        principalTable: "OverWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OWFDS_OverWorkId",
                table: "OWFDS",
                column: "OverWorkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OWFDS");

            migrationBuilder.DropTable(
                name: "Vacations");

            migrationBuilder.DropTable(
                name: "FreeDays");

            migrationBuilder.DropTable(
                name: "OverWorks");
        }
    }
}
