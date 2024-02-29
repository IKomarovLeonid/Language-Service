using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class Initialize_Db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttemptHistories",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<ulong>(nullable: false),
                    total_attempts = table.Column<ulong>(nullable: false),
                    correct_answers = table.Column<ulong>(nullable: false),
                    words_types = table.Column<string>(nullable: false),
                    category = table.Column<string>(nullable: false),
                    attempt_time_sec = table.Column<double>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    updated_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptHistories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Attempts",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    history_id = table.Column<ulong>(nullable: false),
                    word = table.Column<string>(nullable: true),
                    user_translation = table.Column<string>(nullable: true),
                    expected_trasnlation = table.Column<string>(nullable: true),
                    is_correct = table.Column<bool>(nullable: false),
                    attempt_time_sec = table.Column<double>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    updated_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attempts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    type = table.Column<string>(nullable: false),
                    category = table.Column<string>(nullable: false),
                    language_from = table.Column<string>(nullable: false),
                    language_to = table.Column<string>(nullable: false),
                    word = table.Column<string>(nullable: true),
                    translation = table.Column<string>(nullable: true),
                    created_utc = table.Column<DateTime>(nullable: false),
                    updated_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttemptHistories");

            migrationBuilder.DropTable(
                name: "Attempts");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
