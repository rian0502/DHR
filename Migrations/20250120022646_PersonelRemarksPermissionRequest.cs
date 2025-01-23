using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DHR.Migrations
{
    /// <inheritdoc />
    public partial class PersonelRemarksPermissionRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonnelRemarks",
                table: "EmployeePermissionRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonnelRemarks",
                table: "EmployeePermissionRequest");
        }
    }
}
