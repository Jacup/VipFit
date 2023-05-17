using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VipFit.Core.Migrations
{
    /// <inheritdoc />
    public partial class PassEntry1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Passes_PassId",
                table: "Entry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entry",
                table: "Entry");

            migrationBuilder.RenameTable(
                name: "Entry",
                newName: "Entries");

            migrationBuilder.RenameIndex(
                name: "IX_Entry_PassId",
                table: "Entries",
                newName: "IX_Entries_PassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entries",
                table: "Entries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Passes_PassId",
                table: "Entries",
                column: "PassId",
                principalTable: "Passes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Passes_PassId",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entries",
                table: "Entries");

            migrationBuilder.RenameTable(
                name: "Entries",
                newName: "Entry");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_PassId",
                table: "Entry",
                newName: "IX_Entry_PassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entry",
                table: "Entry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Passes_PassId",
                table: "Entry",
                column: "PassId",
                principalTable: "Passes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
