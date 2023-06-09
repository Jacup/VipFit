using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VipFit.Core.Migrations
{
    /// <inheritdoc />
    public partial class ref1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PassTemplates",
                newName: "MonthsDuration");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "PassTemplates",
                newName: "PricePerMonth");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "PassTemplates",
                newName: "EntriesPerMonth");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PassTemplates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PassTemplates");

            migrationBuilder.RenameColumn(
                name: "PricePerMonth",
                table: "PassTemplates",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "MonthsDuration",
                table: "PassTemplates",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "EntriesPerMonth",
                table: "PassTemplates",
                newName: "Duration");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
