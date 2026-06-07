using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Migrations
{
    /// <inheritdoc />
    public partial class available4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShift_Shifts_AvailabilityId",
                table: "EmployeeShift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeShift",
                table: "EmployeeShift");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShift_EmployeesId",
                table: "EmployeeShift");

            migrationBuilder.RenameColumn(
                name: "AvailabilityId",
                table: "EmployeeShift",
                newName: "ShiftsId");

            migrationBuilder.AddColumn<Guid>(
                name: "AvailabilityId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeShift",
                table: "EmployeeShift",
                columns: new[] { "EmployeesId", "ShiftsId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShift_ShiftsId",
                table: "EmployeeShift",
                column: "ShiftsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShift_Shifts_ShiftsId",
                table: "EmployeeShift",
                column: "ShiftsId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShift_Shifts_ShiftsId",
                table: "EmployeeShift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeShift",
                table: "EmployeeShift");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShift_ShiftsId",
                table: "EmployeeShift");

            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "ShiftsId",
                table: "EmployeeShift",
                newName: "AvailabilityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeShift",
                table: "EmployeeShift",
                columns: new[] { "AvailabilityId", "EmployeesId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShift_EmployeesId",
                table: "EmployeeShift",
                column: "EmployeesId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShift_Shifts_AvailabilityId",
                table: "EmployeeShift",
                column: "AvailabilityId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
