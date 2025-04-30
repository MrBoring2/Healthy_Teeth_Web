using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class add_indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApartmentNuber",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Home",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Patients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Patients",
                type: "text",
                nullable: false,
                computedColumnSql: "trim(\"FirstName\" || ' ' || \"MiddleName\" || ' ' || \"LastName\")",
                stored: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Employees",
                type: "text",
                nullable: false,
                computedColumnSql: "trim(\"FirstName\" || ' ' || \"MiddleName\" || ' ' || \"LastName\")",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visits_VisirtTime",
                table: "Visits",
                column: "VisirtTime");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_VisitDate",
                table: "Visits",
                column: "VisitDate");

            migrationBuilder.CreateIndex(
                name: "IX_Services_Title",
                table: "Services",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_FullName",
                table: "Employees",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Login",
                table: "Accounts",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Visits_VisirtTime",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_VisitDate",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Services_Title",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Employees_FullName",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Login",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "ApartmentNuber",
                table: "Patients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Patients",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Home",
                table: "Patients",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Patients",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
