using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class removeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerSheet_Attendance",
                table: "AnswerSheet");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Assignment",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_StuInClass",
                table: "Attendance");

            migrationBuilder.DropTable(
                name: "Assignment");

            //migrationBuilder.DropIndex(
            //    name: "IX_Attendance_AssignmentId",
            //    table: "Attendance");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Attendance");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerSheet_Attendance_AttendDate_StuInClassId",
                table: "AnswerSheet",
                columns: new[] { "AttendDate", "StuInClassId" },
                principalTable: "Attendance",
                principalColumns: new[] { "AttendDate", "StuClassInId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_StuInClass_StuClassInId",
                table: "Attendance",
                column: "StuClassInId",
                principalTable: "StuInClass",
                principalColumn: "StuInClassId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerSheet_Attendance_AttendDate_StuInClassId",
                table: "AnswerSheet");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_StuInClass_StuClassInId",
                table: "Attendance");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentId",
                table: "Attendance",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    AssignmentId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Time = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Assignment_Courses",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_AssignmentId",
                table: "Attendance",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_CourseId",
                table: "Assignment",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerSheet_Attendance",
                table: "AnswerSheet",
                columns: new[] { "AttendDate", "StuInClassId" },
                principalTable: "Attendance",
                principalColumns: new[] { "AttendDate", "StuClassInId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Assignment",
                table: "Attendance",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_StuInClass",
                table: "Attendance",
                column: "StuClassInId",
                principalTable: "StuInClass",
                principalColumn: "StuInClassId");
        }
    }
}
