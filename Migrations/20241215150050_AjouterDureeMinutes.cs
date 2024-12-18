﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace videotheque.Migrations
{
    /// <inheritdoc />
    public partial class AjouterDureeMinutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DureeMinutes",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DureeMinutes",
                table: "Films");
        }
    }
}
