using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oline_Ride_Share_idb_project.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRideTrackModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "RideTracks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RideTracks");
        }
    }
}
