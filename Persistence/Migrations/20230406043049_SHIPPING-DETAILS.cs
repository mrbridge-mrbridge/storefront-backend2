using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SHIPPINGDETAILS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "ShipingDetails",
                newName: "StreetName");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ShipingDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ShipingDetails",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "ShipingDetails");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ShipingDetails");

            migrationBuilder.RenameColumn(
                name: "StreetName",
                table: "ShipingDetails",
                newName: "Location");
        }
    }
}
