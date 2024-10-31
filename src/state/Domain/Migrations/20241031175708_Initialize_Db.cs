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
                    total_attempts = table.Column<ulong>(nullable: false),
                    correct_answers = table.Column<ulong>(nullable: false),
                    words_errors = table.Column<string>(nullable: true),
                    attributes = table.Column<string>(nullable: true),
                    created_utc = table.Column<DateTime>(nullable: false),
                    updated_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptHistories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    attributes = table.Column<string>(nullable: true),
                    word = table.Column<string>(nullable: true),
                    translation = table.Column<string>(nullable: false),
                    conjugation = table.Column<string>(nullable: true),
                    language_type = table.Column<string>(nullable: false),
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
                name: "Words");
        }
    }
}
