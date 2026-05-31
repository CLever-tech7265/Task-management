using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeLevel_LevelId",
                table: "Employees");

            migrationBuilder.AlterColumn<Guid>(
                name: "LevelId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeLevel_LevelId",
                table: "Employees",
                column: "LevelId",
                principalTable: "EmployeeLevel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeLevel_LevelId",
                table: "Employees");

            migrationBuilder.AlterColumn<Guid>(
                name: "LevelId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeLevel_LevelId",
                table: "Employees",
                column: "LevelId",
                principalTable: "EmployeeLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
