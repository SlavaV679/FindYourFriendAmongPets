using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindYourFriendAmongPets.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PetFilesAsListInsteadVOList4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pet_photos",
                table: "pets",
                newName: "pet_files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pet_files",
                table: "pets",
                newName: "pet_photos");
        }
    }
}
