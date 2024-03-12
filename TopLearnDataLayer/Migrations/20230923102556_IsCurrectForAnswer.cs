using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopLearnDataLayer.Migrations
{
    /// <inheritdoc />
    public partial class IsCurrectForAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCurrect",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrect",
                table: "Answers");
        }
    }
}
