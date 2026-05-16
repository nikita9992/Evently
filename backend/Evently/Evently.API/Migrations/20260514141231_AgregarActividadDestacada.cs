using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarActividadDestacada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsDestacada",
                table: "Actividades",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsDestacada",
                table: "Actividades");
        }
    }
}
