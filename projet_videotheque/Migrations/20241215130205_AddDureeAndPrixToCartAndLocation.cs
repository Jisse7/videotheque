using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace videotheque.Migrations
{
    /// <inheritdoc />
    public partial class AddDureeAndPrixToCartAndLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrixLocation",
                table: "Films",
                newName: "Prix72h");

            migrationBuilder.AddColumn<int>(
                name: "DureeLocation",
                table: "LocationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Prix1Semaine",
                table: "Films",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Prix24h",
                table: "Films",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Prix48h",
                table: "Films",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DureeLocation",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PrixLocation",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DureeLocation",
                table: "LocationDetails");

            migrationBuilder.DropColumn(
                name: "Prix1Semaine",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Prix24h",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Prix48h",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "DureeLocation",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "PrixLocation",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Prix72h",
                table: "Films",
                newName: "PrixLocation");
        }
    }
}
