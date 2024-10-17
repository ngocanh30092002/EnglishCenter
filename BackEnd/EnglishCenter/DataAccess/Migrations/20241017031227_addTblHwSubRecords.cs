using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addTblHwSubRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HW_Sub_Records",
                columns: table => new
                {
                    RecordId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmissionId = table.Column<long>(type: "bigint", nullable: false),
                    HwQuesId = table.Column<long>(type: "bigint", nullable: false),
                    HwSubQuesId = table.Column<long>(type: "bigint", nullable: true),
                    SelectedAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HW_Sub_Records", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_HwSubRecord_HwSubmission",
                        column: x => x.SubmissionId,
                        principalTable: "HW_Submission",
                        principalColumn: "SubmissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HW_Sub_Records_SubmissionId",
                table: "HW_Sub_Records",
                column: "SubmissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HW_Sub_Records");
        }
    }
}
