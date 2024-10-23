using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class remove_weekDays_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Weekday",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "Weekdays");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_WeekdayId",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "WeekdayId",
                table: "Schedules",
                newName: "Weekday");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weekday",
                table: "Schedules",
                newName: "WeekdayId");

            migrationBuilder.CreateTable(
                name: "Weekdays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weekdays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_WeekdayId",
                table: "Schedules",
                column: "WeekdayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Weekday",
                table: "Schedules",
                column: "WeekdayId",
                principalTable: "Weekdays",
                principalColumn: "Id");
        }
    }
}
