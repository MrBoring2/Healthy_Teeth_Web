using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_date_time : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeTo",
                table: "Schedules",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeFrom",
                table: "Schedules",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Patients",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TimeTo",
                table: "Schedules",
                type: "interval",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TimeFrom",
                table: "Schedules",
                type: "interval",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Patients",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
