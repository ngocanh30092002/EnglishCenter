using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addAttendanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsAttended = table.Column<bool>(type: "bit", nullable: true),
                    IsPermitted = table.Column<bool>(type: "bit", nullable: true),
                    IsLate = table.Column<bool>(type: "bit", nullable: true),
                    IsLeaved = table.Column<bool>(type: "bit", nullable: true),
                    StuInClassId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_Attendance_StuInClass",
                        column: x => x.StuInClassId,
                        principalTable: "StuInClass",
                        principalColumn: "StuInClassId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StuInClassId",
                table: "Attendances",
                column: "StuInClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");
        }
    }
}
