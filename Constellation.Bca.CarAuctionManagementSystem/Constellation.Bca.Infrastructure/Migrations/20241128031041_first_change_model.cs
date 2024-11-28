using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constellation.Bca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first_change_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NumberOfDoors = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistrationYear = table.Column<int>(type: "INTEGER", nullable: false),
                    StartingBid = table.Column<decimal>(type: "TEXT", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "INTEGER", nullable: false),
                    LoadCapacity = table.Column<double>(type: "REAL", nullable: false),
                    VehicleType = table.Column<int>(type: "INTEGER", nullable: false),
                    UniqueIdentifier = table.Column<string>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                    table.UniqueConstraint("AK_Vehicle_Id_UniqueIdentifier", x => new { x.Id, x.UniqueIdentifier });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
