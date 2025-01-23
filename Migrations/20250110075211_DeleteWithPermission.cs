using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DHR.Migrations
{
    /// <inheritdoc />
    public partial class DeleteWithPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UnitName",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UnitCode",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Units",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "TaxExemptIncomes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TaxExemptIncomes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "SubUnits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubUnits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "SubDepartments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubDepartments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Religions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Religions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Periods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Periods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "JobTitles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "JobTitles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "FormApplication",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FormApplication",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "EmployeeMedicalClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeMedicalClaimCode",
                table: "EmployeeMedicalClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeeMedicalClaims",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "EmployeeLeaveRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeLeaveRequestCode",
                table: "EmployeeLeaveRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeeLeaveRequest",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "EmployeeDependents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeeDependents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "EmployeeBenefits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeeBenefits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Educations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Educations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Divisions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Divisions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Benefits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Benefits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "AttendanceStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AttendanceStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmployeePermissionRequest",
                columns: table => new
                {
                    EmployeePermissionRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeePermissionRequestCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PermissionDays = table.Column<double>(type: "float", nullable: false),
                    PermissionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_EmployeePermissionRequest", x => x.EmployeePermissionRequestId);
                    table.ForeignKey(
                        name: "FK_EmployeePermissionRequest_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePermissionRequest_EmployeeId",
                table: "EmployeePermissionRequest",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeePermissionRequest");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "TaxExemptIncomes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TaxExemptIncomes");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "SubUnits");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubUnits");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "SubDepartments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubDepartments");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Religions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Religions");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "JobTitles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "JobTitles");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "FormApplication");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FormApplication");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "EmployeeMedicalClaims");

            migrationBuilder.DropColumn(
                name: "EmployeeMedicalClaimCode",
                table: "EmployeeMedicalClaims");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmployeeMedicalClaims");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "EmployeeLeaveRequest");

            migrationBuilder.DropColumn(
                name: "EmployeeLeaveRequestCode",
                table: "EmployeeLeaveRequest");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmployeeLeaveRequest");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "EmployeeDependents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmployeeDependents");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "EmployeeBenefits");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmployeeBenefits");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Educations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Educations");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Benefits");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Benefits");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "AttendanceStatus");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AttendanceStatus");

            migrationBuilder.AlterColumn<string>(
                name: "UnitName",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitCode",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
