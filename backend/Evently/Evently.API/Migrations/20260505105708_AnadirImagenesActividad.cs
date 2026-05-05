using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Evently.API.Migrations
{
    /// <inheritdoc />
    public partial class AnadirImagenesActividad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Actividades");

            migrationBuilder.CreateTable(
                name: "ImagenesActividad",
                columns: table => new
                {
                    IdImagen = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdActividad = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagenesActividad", x => x.IdImagen);
                    table.ForeignKey(
                        name: "FK_ImagenesActividad_Actividades_IdActividad",
                        column: x => x.IdActividad,
                        principalTable: "Actividades",
                        principalColumn: "IdActividad",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagenesActividad_IdActividad",
                table: "ImagenesActividad",
                column: "IdActividad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagenesActividad");

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Actividades",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
