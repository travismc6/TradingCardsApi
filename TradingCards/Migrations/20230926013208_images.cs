using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingCards.Migrations
{
    /// <inheritdoc />
    public partial class images : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "CollectionCards",
                newName: "FrontImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "BackImageUrl",
                table: "CollectionCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackImageUrl",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontImageUrl",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackImageUrl",
                table: "CollectionCards");

            migrationBuilder.DropColumn(
                name: "BackImageUrl",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "FrontImageUrl",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "FrontImageUrl",
                table: "CollectionCards",
                newName: "PhotoUrl");
        }
    }
}
