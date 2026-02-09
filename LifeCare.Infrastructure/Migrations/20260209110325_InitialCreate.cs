using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_Status",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Patients",
                type: "text",
                nullable: true,
                defaultValue: "AwaitingTriage",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "AwaitingTriage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Patients",
                type: "text",
                nullable: false,
                defaultValue: "AwaitingTriage",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "AwaitingTriage");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Status",
                table: "Patients",
                column: "Status");
        }
    }
}
