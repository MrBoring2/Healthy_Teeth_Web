using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_connections_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Employee",
                table: "Accounts");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Employee",
                table: "Accounts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Employee",
                table: "Accounts");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Employee",
                table: "Accounts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
