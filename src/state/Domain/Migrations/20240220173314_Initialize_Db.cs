using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class Initialize_Db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    type = table.Column<string>(nullable: false),
                    level = table.Column<string>(nullable: false),
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
                name: "Words");
        }
    }
}
