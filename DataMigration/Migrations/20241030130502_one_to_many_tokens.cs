using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class one_to_many_tokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRefreshTokens",
                table: "EmployeeRefreshTokens");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EmployeeRefreshTokens",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "EmployeeRefreshTokens",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRefreshTokens",
                table: "EmployeeRefreshTokens",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRefreshTokens_EmployeeId",
                table: "EmployeeRefreshTokens",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRefreshTokens",
                table: "EmployeeRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeRefreshTokens_EmployeeId",
                table: "EmployeeRefreshTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EmployeeRefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "EmployeeRefreshTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRefreshTokens",
                table: "EmployeeRefreshTokens",
                column: "EmployeeId");
        }
    }
}
