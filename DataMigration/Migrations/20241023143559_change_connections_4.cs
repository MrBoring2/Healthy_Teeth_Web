using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_connections_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Weekday",
                table: "Schedules");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Weekday",
                table: "Schedules",
                column: "WeekdayId",
                principalTable: "Weekdays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Weekday",
                table: "Schedules");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Weekday",
                table: "Schedules",
                column: "WeekdayId",
                principalTable: "Weekdays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
