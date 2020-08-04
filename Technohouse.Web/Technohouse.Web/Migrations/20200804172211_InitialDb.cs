using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Technohouse.Web.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActingZones",
                columns: table => new
                {
                    ZoneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameZone = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActingZones", x => x.ZoneId);
                });

            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    AgencyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameAgency = table.Column<string>(nullable: false),
                    AddressAgency = table.Column<string>(nullable: false),
                    Cellphone = table.Column<int>(nullable: false),
                    ActingZonesZoneId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.AgencyId);
                    table.ForeignKey(
                        name: "FK_Agencies_ActingZones_ActingZonesZoneId",
                        column: x => x.ActingZonesZoneId,
                        principalTable: "ActingZones",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_ActingZonesZoneId",
                table: "Agencies",
                column: "ActingZonesZoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agencies");

            migrationBuilder.DropTable(
                name: "ActingZones");
        }
    }
}
