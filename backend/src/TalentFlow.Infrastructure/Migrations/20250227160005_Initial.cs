using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "candidate",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    resume_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    second_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_candidate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hr_specialist",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    second_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hr_specialist", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "test_assignment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    assigned_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    submission_deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    assignment_status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    submission_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_test_assignment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vacancies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    department_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    hr_specialist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    opening_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    closing_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vacancies", x => x.id);
                    table.ForeignKey(
                        name: "fk_vacancies_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vacancies_hr_specialist_hr_specialist_id",
                        column: x => x.hr_specialist_id,
                        principalTable: "hr_specialist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recruitment_processes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vacancy_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    candidate_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    test_assignment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    current_stage = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    recruitment_stage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    probation_passed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recruitment_processes", x => x.id);
                    table.ForeignKey(
                        name: "fk_recruitment_processes_candidate_candidate_id",
                        column: x => x.candidate_id,
                        principalTable: "candidate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_recruitment_processes_test_assignment_test_assignment_id",
                        column: x => x.test_assignment_id,
                        principalTable: "test_assignment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_recruitment_processes_vacancies_vacancy_id",
                        column: x => x.vacancy_id,
                        principalTable: "vacancies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_recruitment_processes_candidate_id",
                table: "recruitment_processes",
                column: "candidate_id");

            migrationBuilder.CreateIndex(
                name: "ix_recruitment_processes_test_assignment_id",
                table: "recruitment_processes",
                column: "test_assignment_id");

            migrationBuilder.CreateIndex(
                name: "ix_recruitment_processes_vacancy_id",
                table: "recruitment_processes",
                column: "vacancy_id");

            migrationBuilder.CreateIndex(
                name: "ix_vacancies_department_id",
                table: "vacancies",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_vacancies_hr_specialist_id",
                table: "vacancies",
                column: "hr_specialist_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recruitment_processes");

            migrationBuilder.DropTable(
                name: "candidate");

            migrationBuilder.DropTable(
                name: "test_assignment");

            migrationBuilder.DropTable(
                name: "vacancies");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "hr_specialist");
        }
    }
}
