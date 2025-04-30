using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_gender_length : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Patients",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Employees",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Patients",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Employees",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);
        }
    }
}
