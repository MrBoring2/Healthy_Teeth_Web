using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_passport_length : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_Passport",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Passport",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Patients",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PassportCode",
                table: "Patients",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Patients",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PassportCode",
                table: "Patients",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Passport",
                table: "Patients",
                type: "text",
                nullable: true,
                computedColumnSql: "trim(\"PassportCode\" || ' ' || \"PassportNumber\")",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Passport",
                table: "Patients",
                column: "Passport");
        }
    }
}
