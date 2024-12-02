using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAHAR.Migrations
{
    /// <inheritdoc />
    public partial class FormApplicationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Locations_LocationID",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Companies_CompanyID",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubDepartments_Departments_DepartmentID",
                table: "SubDepartments");

            migrationBuilder.DropTable(
                name: "AppLogs");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "Departments",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_CompanyID",
                table: "Departments",
                newName: "IX_Departments_CompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "SubDepartments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FormApplication",
                columns: table => new
                {
                    IdForm = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PathForm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormApplication", x => x.IdForm);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Locations_LocationID",
                table: "Companies",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Companies_CompanyId",
                table: "Departments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubDepartments_Departments_DepartmentID",
                table: "SubDepartments",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Locations_LocationID",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Companies_CompanyId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubDepartments_Departments_DepartmentID",
                table: "SubDepartments");

            migrationBuilder.DropTable(
                name: "FormApplication");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Departments",
                newName: "CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments",
                newName: "IX_Departments_CompanyID");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "SubDepartments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "Companies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "AppLogs",
                columns: table => new
                {
                    AppLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Params = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogs", x => x.AppLogId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Locations_LocationID",
                table: "Companies",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "LocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Companies_CompanyID",
                table: "Departments",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubDepartments_Departments_DepartmentID",
                table: "SubDepartments",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID");
        }
    }
}
