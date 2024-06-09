using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock2u.Migrations
{
    public partial class remocaoqtdestoqueRestaurante : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "EstoqueRestaurantes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "EstoqueRestaurantes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
