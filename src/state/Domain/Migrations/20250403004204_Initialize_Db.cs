using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class Initialize_Db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameAttempts",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<ulong>(nullable: false),
                    rating_change = table.Column<double>(nullable: false),
                    correct_answers = table.Column<ulong>(nullable: false),
                    total_answers = table.Column<ulong>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    updated_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameAttempts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    is_admin = table.Column<bool>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    user_name = table.Column<string>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    updated_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserStatistics",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<ulong>(nullable: false),
                    user_rating = table.Column<double>(nullable: false),
                    total_attempts = table.Column<ulong>(nullable: false),
                    error_attempts = table.Column<ulong>(nullable: false),
                    last_time_utc = table.Column<DateTime>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    updated_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatistics", x => x.id);
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
                    word_rating = table.Column<double>(nullable: false),
                    language_type = table.Column<string>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    updated_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WordStatistics",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<ulong>(nullable: false),
                    word_id = table.Column<ulong>(nullable: false),
                    correct_answers = table.Column<ulong>(nullable: false),
                    total_answers = table.Column<ulong>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    updated_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordStatistics", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameAttempts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserStatistics");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "WordStatistics");
        }
    }
}
