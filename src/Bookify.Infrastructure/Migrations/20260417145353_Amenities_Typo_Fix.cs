using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Amenities_Typo_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amentities_up_charge_currency",
                table: "bookings",
                newName: "amenities_up_charge_currency");

            migrationBuilder.RenameColumn(
                name: "amentities_up_charge_amount",
                table: "bookings",
                newName: "amenities_up_charge_amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amenities_up_charge_currency",
                table: "bookings",
                newName: "amentities_up_charge_currency");

            migrationBuilder.RenameColumn(
                name: "amenities_up_charge_amount",
                table: "bookings",
                newName: "amentities_up_charge_amount");
        }
    }
}
