using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Migrations
{
    /// <inheritdoc />
    public partial class addTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Shifts_Shift",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Specialization_Specialization",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_Shift",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_Specialization",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Shift",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Tasks");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "Specialization",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "Shifts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialization_TaskId",
                table: "Specialization",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_TaskId",
                table: "Shifts",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Tasks_TaskId",
                table: "Shifts",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialization_Tasks_TaskId",
                table: "Specialization",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Tasks_TaskId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialization_Tasks_TaskId",
                table: "Specialization");

            migrationBuilder.DropIndex(
                name: "IX_Specialization_TaskId",
                table: "Specialization");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_TaskId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Specialization");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Shifts");

            migrationBuilder.AddColumn<Guid>(
                name: "Shift",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Specialization",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Shift",
                table: "Tasks",
                column: "Shift");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Specialization",
                table: "Tasks",
                column: "Specialization");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Shifts_Shift",
                table: "Tasks",
                column: "Shift",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Specialization_Specialization",
                table: "Tasks",
                column: "Specialization",
                principalTable: "Specialization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
