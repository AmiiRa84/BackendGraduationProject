using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mothernotemigupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotherNote",
                table: "TaskResults");

            migrationBuilder.AddColumn<string>(
                name: "MotherNote",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotherNote",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "MotherNote",
                table: "TaskResults",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
