using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class removeTables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerSheet");

            migrationBuilder.DropTable(
                name: "Attendance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    AttendDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StuClassInId = table.Column<long>(type: "bigint", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsAttended = table.Column<bool>(type: "bit", nullable: true),
                    IsLated = table.Column<bool>(type: "bit", nullable: true),
                    IsLeaved = table.Column<bool>(type: "bit", nullable: true),
                    IsPermited = table.Column<bool>(type: "bit", nullable: true),
                    LessionNum = table.Column<int>(type: "int", nullable: true),
                    StatusAssignment = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => new { x.AttendDate, x.StuClassInId });
                    table.ForeignKey(
                        name: "FK_Attendance_StuInClass_StuClassInId",
                        column: x => x.StuClassInId,
                        principalTable: "StuInClass",
                        principalColumn: "StuInClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerSheet",
                columns: table => new
                {
                    AnswerSheetId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StuInClassId = table.Column<long>(type: "bigint", nullable: true),
                    AnswerString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CorrectNum = table.Column<int>(type: "int", nullable: true),
                    FalseNum = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerSheet", x => x.AnswerSheetId);
                    table.ForeignKey(
                        name: "FK_AnswerSheet_Attendance_AttendDate_StuInClassId",
                        columns: x => new { x.AttendDate, x.StuInClassId },
                        principalTable: "Attendance",
                        principalColumns: new[] { "AttendDate", "StuClassInId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerSheet_AttendDate_StuInClassId",
                table: "AnswerSheet",
                columns: new[] { "AttendDate", "StuInClassId" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StuClassInId",
                table: "Attendance",
                column: "StuClassInId");
        }
    }
}
