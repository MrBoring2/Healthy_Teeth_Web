using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_connections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Employee",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Role",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Specialization",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Specialization",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_Employee",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_Patient",
                table: "Visits");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Employee",
                table: "Accounts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Role",
                table: "Accounts",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Specialization",
                table: "Employees",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Specialization",
                table: "Services",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_Employee",
                table: "Visits",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_Patient",
                table: "Visits",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Employee",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Role",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Specialization",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Specialization",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_Employee",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_Patient",
                table: "Visits");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Employee",
                table: "Accounts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Role",
                table: "Accounts",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Specialization",
                table: "Employees",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Specialization",
                table: "Services",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_Employee",
                table: "Visits",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_Patient",
                table: "Visits",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
