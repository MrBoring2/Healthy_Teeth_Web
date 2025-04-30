using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_visit_Date : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "VisitDate",
                table: "Visits",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "VisirtTime",
                table: "Visits",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "VisitDate",
                table: "Visits",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "VisirtTime",
                table: "Visits",
                type: "interval",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");
        }
    }
}
