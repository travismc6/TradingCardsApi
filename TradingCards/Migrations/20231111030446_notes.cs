using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingCards.Migrations
{
    /// <inheritdoc />
    public partial class notes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Cards",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_CardSets_BrandId",
                table: "CardSets",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardSets_Brands_BrandId",
                table: "CardSets",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardSets_Brands_BrandId",
                table: "CardSets");

            migrationBuilder.DropIndex(
                name: "IX_CardSets_BrandId",
                table: "CardSets");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Cards",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
