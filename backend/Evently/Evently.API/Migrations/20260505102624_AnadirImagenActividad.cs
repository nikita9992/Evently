using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.API.Migrations
{
    /// <inheritdoc />
    public partial class AnadirImagenActividad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Actividades",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Actividades");
        }
    }
}
