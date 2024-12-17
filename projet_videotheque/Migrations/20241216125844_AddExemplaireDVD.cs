using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace videotheque.Migrations
{
    /// <inheritdoc />
    public partial class AddExemplaireDVD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExemplaireDVDId",
                table: "LocationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExemplairesDVD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeBarre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EstDansStock = table.Column<bool>(type: "bit", nullable: false),
                    FilmId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExemplairesDVD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExemplairesDVD_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationDetails_ExemplaireDVDId",
                table: "LocationDetails",
                column: "ExemplaireDVDId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExemplairesDVD_CodeBarre",
                table: "ExemplairesDVD",
                column: "CodeBarre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExemplairesDVD_FilmId",
                table: "ExemplairesDVD",
                column: "FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationDetails_ExemplairesDVD_ExemplaireDVDId",
                table: "LocationDetails",
                column: "ExemplaireDVDId",
                principalTable: "ExemplairesDVD",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationDetails_ExemplairesDVD_ExemplaireDVDId",
                table: "LocationDetails");

            migrationBuilder.DropTable(
                name: "ExemplairesDVD");

            migrationBuilder.DropIndex(
                name: "IX_LocationDetails_ExemplaireDVDId",
                table: "LocationDetails");

            migrationBuilder.DropColumn(
                name: "ExemplaireDVDId",
                table: "LocationDetails");
        }
    }
}
