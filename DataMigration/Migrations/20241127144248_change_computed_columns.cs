using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_computed_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Patients",
                type: "text",
                nullable: true,
                computedColumnSql: "trim(\"FirstName\" || ' ' || \"LastName\" || ' ' || \"MiddleName\")",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComputedColumnSql: "trim(\"FirstName\" || ' ' || \"MiddleName\" || ' ' || \"LastName\")",
                oldStored: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Employees",
                type: "text",
                nullable: false,
                computedColumnSql: "trim(\"FirstName\" || ' ' || \"LastName\" || ' ' || \"MiddleName\")",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "trim(\"FirstName\" || ' ' || \"MiddleName\" || ' ' || \"LastName\")",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Patients",
                type: "text",
                nullable: true,
                computedColumnSql: "trim(\"FirstName\" || ' ' || \"MiddleName\" || ' ' || \"LastName\")",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComputedColumnSql: "trim(\"FirstName\" || ' ' || \"LastName\" || ' ' || \"MiddleName\")",
                oldStored: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Employees",
                type: "text",
                nullable: false,
                computedColumnSql: "trim(\"FirstName\" || ' ' || \"MiddleName\" || ' ' || \"LastName\")",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "trim(\"FirstName\" || ' ' || \"LastName\" || ' ' || \"MiddleName\")",
                oldStored: true);
        }
    }
}
