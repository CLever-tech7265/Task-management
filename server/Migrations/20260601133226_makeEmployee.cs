using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Migrations
{
    /// <inheritdoc />
    public partial class makeEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeLevel_Level",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSpecialization_Specialization_SpecId",
                table: "EmployeeSpecialization");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Level",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Spec",
                table: "Specialization",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SpecId",
                table: "EmployeeSpecialization",
                newName: "SpecsId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSpecialization_SpecId",
                table: "EmployeeSpecialization",
                newName: "IX_EmployeeSpecialization_SpecsId");

            migrationBuilder.AddColumn<Guid>(
                name: "LevelId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PeopleId",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shift = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Specialization = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Shifts_Shift",
                        column: x => x.Shift,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Specialization_Specialization",
                        column: x => x.Specialization,
                        principalTable: "Specialization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LevelId",
                table: "Employees",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Shift",
                table: "Tasks",
                column: "Shift");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Specialization",
                table: "Tasks",
                column: "Specialization");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeLevel_LevelId",
                table: "Employees",
                column: "LevelId",
                principalTable: "EmployeeLevel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSpecialization_Specialization_SpecsId",
                table: "EmployeeSpecialization",
                column: "SpecsId",
                principalTable: "Specialization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeLevel_LevelId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSpecialization_Specialization_SpecsId",
                table: "EmployeeSpecialization");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Employees_LevelId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Specialization",
                newName: "Spec");

            migrationBuilder.RenameColumn(
                name: "SpecsId",
                table: "EmployeeSpecialization",
                newName: "SpecId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSpecialization_SpecsId",
                table: "EmployeeSpecialization",
                newName: "IX_EmployeeSpecialization_SpecId");

            migrationBuilder.AddColumn<Guid>(
                name: "Level",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Level",
                table: "Employees",
                column: "Level");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeLevel_Level",
                table: "Employees",
                column: "Level",
                principalTable: "EmployeeLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSpecialization_Specialization_SpecId",
                table: "EmployeeSpecialization",
                column: "SpecId",
                principalTable: "Specialization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
