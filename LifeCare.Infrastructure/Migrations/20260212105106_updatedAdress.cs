using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedAdress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Patients",
                newName: "County");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Patients",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Patients",
                newName: "SubCounty");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PhoneNumber",
                table: "Patients",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Status",
                table: "Patients",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Status_CreatedAt",
                table: "Patients",
                columns: new[] { "Status", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_PhoneNumber",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Status",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Status_CreatedAt",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "SubCounty",
                table: "Patients",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "County",
                table: "Patients",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Patients",
                newName: "State");
        }
    }
}
