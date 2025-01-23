using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DHR.Migrations
{
    /// <inheritdoc />
    public partial class PermissionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PermissionType",
                table: "EmployeePermissionRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionType",
                table: "EmployeePermissionRequest");
        }
    }
}
