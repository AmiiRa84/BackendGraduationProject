using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FinalYarabB2aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Parents_ParentId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Recommendation",
                table: "Reports",
                newName: "Content");

            migrationBuilder.AddColumn<int>(
                name: "ChildId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId1",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalMoves = table.Column<int>(type: "int", nullable: true),
                    TimeTaken = table.Column<int>(type: "int", nullable: true),
                    RoundsCount = table.Column<int>(type: "int", nullable: true),
                    MotherNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpecialistTaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskResults_Tasks_SpecialistTaskId",
                        column: x => x.SpecialistTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ChildId",
                table: "Reports",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ParentId1",
                table: "Reports",
                column: "ParentId1");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResults_SpecialistTaskId",
                table: "TaskResults",
                column: "SpecialistTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Children_ChildId",
                table: "Reports",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Parents_ParentId",
                table: "Reports",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Parents_ParentId1",
                table: "Reports",
                column: "ParentId1",
                principalTable: "Parents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Children_ChildId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Parents_ParentId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Parents_ParentId1",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "TaskResults");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ChildId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ParentId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ParentId1",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Reports",
                newName: "Recommendation");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Parents_ParentId",
                table: "Reports",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
