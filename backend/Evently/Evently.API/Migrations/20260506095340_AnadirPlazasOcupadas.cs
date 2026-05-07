using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.API.Migrations
{
    /// <inheritdoc />
    public partial class AnadirPlazasOcupadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlazasOcupadas",
                table: "Actividades",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlazasOcupadas",
                table: "Actividades");
        }
    }
}
