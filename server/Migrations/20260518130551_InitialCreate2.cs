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
                name: "FK_EmployeeSpecialization_Employees_employeesId",
                table: "EmployeeSpecialization");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftSpecialization_Shifts_shiftId",
                table: "ShiftSpecialization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftSpecialization",
                table: "ShiftSpecialization");

            migrationBuilder.DropIndex(
                name: "IX_ShiftSpecialization_shiftId",
                table: "ShiftSpecialization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeSpecialization",
                table: "EmployeeSpecialization");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSpecialization_employeesId",
                table: "EmployeeSpecialization");

            migrationBuilder.RenameColumn(
                name: "shiftId",
                table: "ShiftSpecialization",
                newName: "ShiftsId");

            migrationBuilder.RenameColumn(
                name: "employeesId",
                table: "EmployeeSpecialization",
                newName: "EmployeesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftSpecialization",
                table: "ShiftSpecialization",
                columns: new[] { "ShiftsId", "SpecsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeSpecialization",
                table: "EmployeeSpecialization",
                columns: new[] { "EmployeesId", "SpecId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSpecialization_SpecsId",
                table: "ShiftSpecialization",
                column: "SpecsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSpecialization_SpecId",
                table: "EmployeeSpecialization",
                column: "SpecId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSpecialization_Employees_EmployeesId",
                table: "EmployeeSpecialization",
                column: "EmployeesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSpecialization_Shifts_ShiftsId",
                table: "ShiftSpecialization",
                column: "ShiftsId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSpecialization_Employees_EmployeesId",
                table: "EmployeeSpecialization");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftSpecialization_Shifts_ShiftsId",
                table: "ShiftSpecialization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftSpecialization",
                table: "ShiftSpecialization");

            migrationBuilder.DropIndex(
                name: "IX_ShiftSpecialization_SpecsId",
                table: "ShiftSpecialization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeSpecialization",
                table: "EmployeeSpecialization");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSpecialization_SpecId",
                table: "EmployeeSpecialization");

            migrationBuilder.RenameColumn(
                name: "ShiftsId",
                table: "ShiftSpecialization",
                newName: "shiftId");

            migrationBuilder.RenameColumn(
                name: "EmployeesId",
                table: "EmployeeSpecialization",
                newName: "employeesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftSpecialization",
                table: "ShiftSpecialization",
                columns: new[] { "SpecsId", "shiftId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeSpecialization",
                table: "EmployeeSpecialization",
                columns: new[] { "SpecId", "employeesId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSpecialization_shiftId",
                table: "ShiftSpecialization",
                column: "shiftId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSpecialization_employeesId",
                table: "EmployeeSpecialization",
                column: "employeesId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSpecialization_Employees_employeesId",
                table: "EmployeeSpecialization",
                column: "employeesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSpecialization_Shifts_shiftId",
                table: "ShiftSpecialization",
                column: "shiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
