using Microsoft.EntityFrameworkCore.Migrations;

namespace ProyectofinalLabo4.Migrations
{
    public partial class FotoJugadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "foto",
                table: "Jugadores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "foto",
                table: "Jugadores");
        }
    }
}
