using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BochaStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class actualizacion_tablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "IdCategoria", "Nombre", "ProductoIdProducto" },
                values: new object[] { 1, "Camisetas", null });

            migrationBuilder.InsertData(
                table: "Marcas",
                columns: new[] { "IdMarca", "Nombre", "ProductoIdProducto" },
                values: new object[] { 1, "Adidas", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Marcas",
                keyColumn: "IdMarca",
                keyValue: 1);
        }
    }
}
