using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.API.Migrations
{
    /// <inheritdoc />
    public partial class AnadirCiudadYUbicacionActividad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Actividades",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                table: "Actividades",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Actividades");

            migrationBuilder.DropColumn(
                name: "Ubicacion",
                table: "Actividades");
        }
    }
}
