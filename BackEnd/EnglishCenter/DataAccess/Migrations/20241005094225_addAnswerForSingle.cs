using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addAnswerForSingle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerA",
                table: "Ques_RC_Single");

            migrationBuilder.DropColumn(
                name: "AnswerB",
                table: "Ques_RC_Single");

            migrationBuilder.DropColumn(
                name: "AnswerC",
                table: "Ques_RC_Single");

            migrationBuilder.DropColumn(
                name: "AnswerD",
                table: "Ques_RC_Single");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Ques_RC_Single");

            migrationBuilder.DropColumn(
                name: "Question",
                table: "Ques_RC_Single");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Ques_RC_Single",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Ques_RC_Single",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AnswerRcSingle",
                columns: table => new
                {
                    AnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerRcSingle", x => x.AnswerId);
                });

            migrationBuilder.CreateTable(
                name: "Sub_RC_Single",
                columns: table => new
                {
                    SubId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreQuesId = table.Column<long>(type: "bigint", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerA = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerD = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sub_RC_Single", x => x.SubId);
                    table.ForeignKey(
                        name: "FK_Sub_Ques_RC_Answer_Single",
                        column: x => x.AnswerId,
                        principalTable: "AnswerRcSingle",
                        principalColumn: "AnswerId");
                    table.ForeignKey(
                        name: "FK_Sub_RC_Double_Ques_RC_Single",
                        column: x => x.PreQuesId,
                        principalTable: "Ques_RC_Single",
                        principalColumn: "QuesId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sub_RC_Single_AnswerId",
                table: "Sub_RC_Single",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sub_RC_Single_PreQuesId",
                table: "Sub_RC_Single",
                column: "PreQuesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sub_RC_Single");

            migrationBuilder.DropTable(
                name: "AnswerRcSingle");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Ques_RC_Single");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Ques_RC_Single");

            migrationBuilder.AddColumn<string>(
                name: "AnswerA",
                table: "Ques_RC_Single",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnswerB",
                table: "Ques_RC_Single",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnswerC",
                table: "Ques_RC_Single",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnswerD",
                table: "Ques_RC_Single",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Ques_RC_Single",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Ques_RC_Single",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
