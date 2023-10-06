using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingCards.Migrations
{
    /// <inheritdoc />
    public partial class publicidproperclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackImagePublicId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "FrontImagePublicId",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "BackImagePublicId",
                table: "CollectionCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontImagePublicId",
                table: "CollectionCards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackImagePublicId",
                table: "CollectionCards");

            migrationBuilder.DropColumn(
                name: "FrontImagePublicId",
                table: "CollectionCards");

            migrationBuilder.AddColumn<string>(
                name: "BackImagePublicId",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontImagePublicId",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
