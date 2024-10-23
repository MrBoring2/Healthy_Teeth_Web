using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigration.Migrations
{
    /// <inheritdoc />
    public partial class change_connections_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceToVisits_Services_ServiceId",
                table: "ServiceToVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceToVisits_Visits_VisitId",
                table: "ServiceToVisits");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceToVisits_Services_ServiceId",
                table: "ServiceToVisits",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceToVisits_Visits_VisitId",
                table: "ServiceToVisits",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceToVisits_Services_ServiceId",
                table: "ServiceToVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceToVisits_Visits_VisitId",
                table: "ServiceToVisits");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceToVisits_Services_ServiceId",
                table: "ServiceToVisits",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceToVisits_Visits_VisitId",
                table: "ServiceToVisits",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
