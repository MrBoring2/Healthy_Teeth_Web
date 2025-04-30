using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class add_patient_index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Patients_FullName",
                table: "Patients",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Passport",
                table: "Patients",
                column: "Passport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_FullName",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Passport",
                table: "Patients");
        }
    }
}
