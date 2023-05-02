using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MERCHANT_LOCATION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "user",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "user");

            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "user");
        }
    }
}
