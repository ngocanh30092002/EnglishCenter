using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class Init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fee = table.Column<decimal>(type: "money", nullable: false),
                    EntryPoint = table.Column<int>(type: "int", nullable: false),
                    StandardPoint = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course_ID", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToeicExams",
                columns: table => new
                {
                    ToeicId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SerialNum = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToeicExams", x => x.ToeicId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RegisteredNum = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_Courses_Classes",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Toeic_Part_1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToeicId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    ImageLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Audio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Part_1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toeic_Part_1_ToeicExams",
                        column: x => x.ToeicId,
                        principalTable: "ToeicExams",
                        principalColumn: "ToeicId");
                });

            migrationBuilder.CreateTable(
                name: "Toeic_Part_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToeicId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Audio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Part_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toeic_Part_2_ToeicExams",
                        column: x => x.ToeicId,
                        principalTable: "ToeicExams",
                        principalColumn: "ToeicId");
                });

            migrationBuilder.CreateTable(
                name: "Toeic_Part_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToeicId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    ImageLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Audio = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Part_3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toeic_Part_3_ToeicExams",
                        column: x => x.ToeicId,
                        principalTable: "ToeicExams",
                        principalColumn: "ToeicId");
                });

            migrationBuilder.CreateTable(
                name: "Toeic_Part_4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToeicId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    ImageLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Audio = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Part_4", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toeic_Part_4_ToeicExams",
                        column: x => x.ToeicId,
                        principalTable: "ToeicExams",
                        principalColumn: "ToeicId");
                });

            migrationBuilder.CreateTable(
                name: "Toeic_Part_5",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToeicId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Part_5", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toeic_Part_5_ToeicExams",
                        column: x => x.ToeicId,
                        principalTable: "ToeicExams",
                        principalColumn: "ToeicId");
                });

            migrationBuilder.CreateTable(
                name: "Toeic_Part_6",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToeicId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    ImageLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Part_6", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toeic_Part_6_ToeicExams",
                        column: x => x.ToeicId,
                        principalTable: "ToeicExams",
                        principalColumn: "ToeicId");
                });

            migrationBuilder.CreateTable(
                name: "Toeic_Part_7",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToeicId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    ImageLink_1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageLink_2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageLink_3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Part_7", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toeic_Part_7_ToeicExams",
                        column: x => x.ToeicId,
                        principalTable: "ToeicExams",
                        principalColumn: "ToeicId");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student_ID", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_StudentUser",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubQuestion3",
                columns: table => new
                {
                    SubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreId = table.Column<int>(type: "int", nullable: false),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubQuestion3", x => new { x.SubId, x.PreId });
                    table.ForeignKey(
                        name: "FK_SubQuestion3_Toeic_Part_3",
                        column: x => x.PreId,
                        principalTable: "Toeic_Part_3",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubQuestion4",
                columns: table => new
                {
                    SubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreId = table.Column<int>(type: "int", nullable: false),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubQuestion4", x => new { x.SubId, x.PreId });
                    table.ForeignKey(
                        name: "FK_SubQuestion4_Toeic_Part_4",
                        column: x => x.PreId,
                        principalTable: "Toeic_Part_4",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubQuestion6",
                columns: table => new
                {
                    SubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreId = table.Column<int>(type: "int", nullable: true),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubQuestion6", x => x.SubId);
                    table.ForeignKey(
                        name: "FK_SubQuestion6_Toeic_Part_6",
                        column: x => x.PreId,
                        principalTable: "Toeic_Part_6",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubQuestion7",
                columns: table => new
                {
                    SubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreId = table.Column<int>(type: "int", nullable: true),
                    NumNo = table.Column<int>(type: "int", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubQuestion7", x => x.SubId);
                    table.ForeignKey(
                        name: "FK_SubQuestion7_Toeic_Part_7",
                        column: x => x.PreId,
                        principalTable: "Toeic_Part_7",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    EnrollDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => new { x.UserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_Enrollment_Courses",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_Enrollment_Students",
                        column: x => x.UserId,
                        principalTable: "Students",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PreExamScores",
                columns: table => new
                {
                    PreScoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    EntrancePoint = table.Column<int>(type: "int", nullable: true),
                    MidtermPoint = table.Column<int>(type: "int", nullable: true),
                    FinalPoint = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreExamScores_1", x => x.PreScoreId);
                    table.ForeignKey(
                        name: "FK_PreExamScores_Courses",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_PreExamScores_Students",
                        column: x => x.UserId,
                        principalTable: "Students",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "StudentInClass",
                columns: table => new
                {
                    StuClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    PreScoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInClass_1", x => x.StuClassId);
                    table.ForeignKey(
                        name: "FK_StudentInClass_PreExamScores",
                        column: x => x.PreScoreId,
                        principalTable: "PreExamScores",
                        principalColumn: "PreScoreId");
                });

            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    AttendDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    StuClassId = table.Column<int>(type: "int", nullable: false),
                    IsAttended = table.Column<bool>(type: "bit", nullable: true),
                    IsPermited = table.Column<bool>(type: "bit", nullable: true),
                    IsLated = table.Column<bool>(type: "bit", nullable: true),
                    IsLeaved = table.Column<bool>(type: "bit", nullable: true),
                    StatusAssignment = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    LinkAssignment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => new { x.AttendDate, x.StuClassId });
                    table.ForeignKey(
                        name: "FK_Attendance_StudentInClass",
                        column: x => x.StuClassId,
                        principalTable: "StudentInClass",
                        principalColumn: "StuClassId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StuClassId",
                table: "Attendance",
                column: "StuClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseId",
                table: "Classes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseId",
                table: "Enrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PreExamScores_CourseId",
                table: "PreExamScores",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PreExamScores_UserId",
                table: "PreExamScores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInClass",
                table: "StudentInClass",
                column: "PreScoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubQuestion3_PreId",
                table: "SubQuestion3",
                column: "PreId");

            migrationBuilder.CreateIndex(
                name: "IX_SubQuestion4_PreId",
                table: "SubQuestion4",
                column: "PreId");

            migrationBuilder.CreateIndex(
                name: "IX_SubQuestion6_PreId",
                table: "SubQuestion6",
                column: "PreId");

            migrationBuilder.CreateIndex(
                name: "IX_SubQuestion7_PreId",
                table: "SubQuestion7",
                column: "PreId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Part_1_ToeicId",
                table: "Toeic_Part_1",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Part_2_ToeicId",
                table: "Toeic_Part_2",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Part_3_ToeicId",
                table: "Toeic_Part_3",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Part_4_ToeicId",
                table: "Toeic_Part_4",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Part_5_ToeicId",
                table: "Toeic_Part_5",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Part_6_ToeicId",
                table: "Toeic_Part_6",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Part_7_ToeicId",
                table: "Toeic_Part_7",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendance");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SubQuestion3");

            migrationBuilder.DropTable(
                name: "SubQuestion4");

            migrationBuilder.DropTable(
                name: "SubQuestion6");

            migrationBuilder.DropTable(
                name: "SubQuestion7");

            migrationBuilder.DropTable(
                name: "Toeic_Part_1");

            migrationBuilder.DropTable(
                name: "Toeic_Part_2");

            migrationBuilder.DropTable(
                name: "Toeic_Part_5");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "StudentInClass");

            migrationBuilder.DropTable(
                name: "Toeic_Part_3");

            migrationBuilder.DropTable(
                name: "Toeic_Part_4");

            migrationBuilder.DropTable(
                name: "Toeic_Part_6");

            migrationBuilder.DropTable(
                name: "Toeic_Part_7");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "PreExamScores");

            migrationBuilder.DropTable(
                name: "ToeicExams");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
