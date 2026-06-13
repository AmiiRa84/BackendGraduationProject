using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReportsUpdateFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Parents_ParentId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ParentId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ParentId1",
                table: "Reports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId1",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ParentId1",
                table: "Reports",
                column: "ParentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Parents_ParentId1",
                table: "Reports",
                column: "ParentId1",
                principalTable: "Parents",
                principalColumn: "Id");
        }
    }
}
