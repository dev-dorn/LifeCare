using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GetRecentAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientStatusHistory");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "PatientStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ChangedBy = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientStatusHistory", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PatientStatusHistory_ChangedAt",
                table: "PatientStatusHistory",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PatientStatusHistory_PatientId",
                table: "PatientStatusHistory",
                column: "PatientId");
        }
    }
}
