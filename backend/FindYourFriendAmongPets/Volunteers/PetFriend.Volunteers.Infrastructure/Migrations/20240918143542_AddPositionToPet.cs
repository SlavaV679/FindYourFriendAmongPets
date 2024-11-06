using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindYourFriendAmongPets.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionToPet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "pets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "position",
                table: "pets");
        }
    }
}
