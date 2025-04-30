using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_gender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Employees",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Employees",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
