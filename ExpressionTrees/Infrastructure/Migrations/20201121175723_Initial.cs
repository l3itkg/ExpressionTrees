using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StaffCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsHighest = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsOfficial = table.Column<bool>(type: "bit", nullable: false),
                    StaffCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_StaffCategories_StaffCategoryId",
                        column: x => x.StaffCategoryId,
                        principalTable: "StaffCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Id",
                table: "Staff",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_StaffCategoryId",
                table: "Staff",
                column: "StaffCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCategories_Id",
                table: "StaffCategories",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "StaffCategories");
        }
    }
}
