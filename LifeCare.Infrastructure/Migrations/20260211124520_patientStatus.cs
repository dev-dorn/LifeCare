using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class patientStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ChangedBy = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientStatusHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientStatusHistory_ChangedAt",
                table: "PatientStatusHistory",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PatientStatusHistory_PatientId",
                table: "PatientStatusHistory",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientStatusHistory");
        }
    }
}
