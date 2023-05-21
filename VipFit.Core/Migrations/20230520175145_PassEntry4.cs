using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VipFit.Core.Migrations
{
    /// <inheritdoc />
    public partial class PassEntry4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Pass_PassId",
                table: "Entry");

            migrationBuilder.DropForeignKey(
                name: "FK_Pass_Clients_ClientId",
                table: "Pass");

            migrationBuilder.DropForeignKey(
                name: "FK_Pass_PassTemplate_PassTemplateId",
                table: "Pass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pass",
                table: "Pass");

            migrationBuilder.RenameTable(
                name: "Pass",
                newName: "Passes");

            migrationBuilder.RenameIndex(
                name: "IX_Pass_PassTemplateId",
                table: "Passes",
                newName: "IX_Passes_PassTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Pass_ClientId",
                table: "Passes",
                newName: "IX_Passes_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passes",
                table: "Passes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Passes_PassId",
                table: "Entry",
                column: "PassId",
                principalTable: "Passes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passes_Clients_ClientId",
                table: "Passes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passes_PassTemplate_PassTemplateId",
                table: "Passes",
                column: "PassTemplateId",
                principalTable: "PassTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Passes_PassId",
                table: "Entry");

            migrationBuilder.DropForeignKey(
                name: "FK_Passes_Clients_ClientId",
                table: "Passes");

            migrationBuilder.DropForeignKey(
                name: "FK_Passes_PassTemplate_PassTemplateId",
                table: "Passes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passes",
                table: "Passes");

            migrationBuilder.RenameTable(
                name: "Passes",
                newName: "Pass");

            migrationBuilder.RenameIndex(
                name: "IX_Passes_PassTemplateId",
                table: "Pass",
                newName: "IX_Pass_PassTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Passes_ClientId",
                table: "Pass",
                newName: "IX_Pass_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pass",
                table: "Pass",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Pass_PassId",
                table: "Entry",
                column: "PassId",
                principalTable: "Pass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pass_Clients_ClientId",
                table: "Pass",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pass_PassTemplate_PassTemplateId",
                table: "Pass",
                column: "PassTemplateId",
                principalTable: "PassTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
