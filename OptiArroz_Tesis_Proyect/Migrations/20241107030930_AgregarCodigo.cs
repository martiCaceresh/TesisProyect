using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiArroz_Tesis_Proyect.Migrations
{
    public partial class AgregarCodigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "RiceSacksOutputs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "RiceSacksDevolutions",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "RiceLots",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "RiceSacksOutputs");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "RiceSacksDevolutions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "RiceLots");
        }
    }
}
