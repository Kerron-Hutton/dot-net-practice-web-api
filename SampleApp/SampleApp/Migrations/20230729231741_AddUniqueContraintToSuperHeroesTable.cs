using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueContraintToSuperHeroesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SuperHeroes_Name",
                table: "SuperHeroes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SuperHeroes_Name",
                table: "SuperHeroes");
        }
    }
}
