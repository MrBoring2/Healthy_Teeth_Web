using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class update_comput_passport_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_Passport",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Passport",
                table: "Patients");
        }
    }
}
