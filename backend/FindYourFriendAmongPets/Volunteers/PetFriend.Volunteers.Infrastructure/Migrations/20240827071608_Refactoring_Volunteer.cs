using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindYourFriendAmongPets.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Refactoring_Volunteer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "count_pets_healing",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "count_pets_looking_for_home",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "count_pets_realized",
                table: "volunteers");

            migrationBuilder.RenameColumn(
                name: "details",
                table: "pets",
                newName: "requisite_details");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "requisite_details",
                table: "pets",
                newName: "details");

            migrationBuilder.AddColumn<int>(
                name: "count_pets_healing",
                table: "volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "count_pets_looking_for_home",
                table: "volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "count_pets_realized",
                table: "volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
