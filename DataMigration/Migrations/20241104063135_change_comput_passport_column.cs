using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_comput_passport_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Passport",
                table: "Patients",
                type: "text",
                nullable: true,
                computedColumnSql: "trim(\"PassportCode\" || ' ' || \"PassportNumber\")",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComputedColumnSql: "trim(\"PassportNumber\" || ' ' || \"PassportCode\")",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Passport",
                table: "Patients",
                type: "text",
                nullable: true,
                computedColumnSql: "trim(\"PassportNumber\" || ' ' || \"PassportCode\")",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComputedColumnSql: "trim(\"PassportCode\" || ' ' || \"PassportNumber\")",
                oldStored: true);
        }
    }
}
