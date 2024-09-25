using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class deleteStuInClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_StuInClass",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "StuInClass");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StuInClassId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "StuInClassId",
                table: "Attendances");

            migrationBuilder.AddColumn<long>(
                name: "ScoreHisId",
                table: "Enrollment",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment",
                table: "Enrollment",
                column: "ScoreHisId",
                unique: true,
                filter: "[ScoreHisId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_ScoreHis",
                table: "Enrollment",
                column: "ScoreHisId",
                principalTable: "ScoreHistory",
                principalColumn: "ScoreHisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_ScoreHis",
                table: "Enrollment");

            migrationBuilder.DropIndex(
                name: "IX_Enrollment",
                table: "Enrollment");

            migrationBuilder.DropColumn(
                name: "ScoreHisId",
                table: "Enrollment");

            migrationBuilder.AddColumn<long>(
                name: "StuInClassId",
                table: "Attendances",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "StuInClass",
                columns: table => new
                {
                    StuInClassId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ScoreHisId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StuInClass", x => x.StuInClassId);
                    table.ForeignKey(
                        name: "FK_StuInClass_Classes",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId");
                    table.ForeignKey(
                        name: "FK_StuInClass_ScoreHistory",
                        column: x => x.ScoreHisId,
                        principalTable: "ScoreHistory",
                        principalColumn: "ScoreHisId");
                    table.ForeignKey(
                        name: "FK_StuInClass_Students",
                        column: x => x.UserId,
                        principalTable: "Students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StuInClassId",
                table: "Attendances",
                column: "StuInClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StuInClass",
                table: "StuInClass",
                column: "ScoreHisId",
                unique: true,
                filter: "[ScoreHisId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StuInClass_ClassId",
                table: "StuInClass",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StuInClass_UserId",
                table: "StuInClass",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_StuInClass",
                table: "Attendances",
                column: "StuInClassId",
                principalTable: "StuInClass",
                principalColumn: "StuInClassId");
        }
    }
}
