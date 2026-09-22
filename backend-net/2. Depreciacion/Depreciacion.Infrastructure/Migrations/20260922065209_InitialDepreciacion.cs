using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depreciacion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDepreciacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    ValorCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaAdquisicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCalculo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorResidual = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Detalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivoId = table.Column<int>(type: "int", nullable: false),
                    NumeroPeriodo = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MesesPeriodo = table.Column<int>(type: "int", nullable: false),
                    DepreciacionPeriodo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepreciacionAcumulada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorLibros = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Detalles_Activos_ActivoId",
                        column: x => x.ActivoId,
                        principalTable: "Activos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Detalles_ActivoId",
                table: "Detalles",
                column: "ActivoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detalles");

            migrationBuilder.DropTable(
                name: "Activos");
        }
    }
}
