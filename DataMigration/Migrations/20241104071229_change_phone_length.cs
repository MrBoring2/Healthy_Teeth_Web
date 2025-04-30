using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_phone_length : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Patients",
                type: "character varying(18)",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employees",
                type: "character varying(18)",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Patients",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(18)",
                oldMaxLength: 18);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employees",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(18)",
                oldMaxLength: 18);
        }
    }
}
