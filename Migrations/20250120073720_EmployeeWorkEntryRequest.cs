using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DHR.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeWorkEntryRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeWorkEntryRequests",
                columns: table => new
                {
                    EmployeeWorkEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeWorkEntryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkDate = table.Column<DateOnly>(type: "date", nullable: true),
                    WorkStartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    WorkEndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    WorkReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonnelRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkEntryRequests", x => x.EmployeeWorkEntryId);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkEntryRequests_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkEntryRequests_EmployeeId",
                table: "EmployeeWorkEntryRequests",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeWorkEntryRequests");
        }
    }
}
