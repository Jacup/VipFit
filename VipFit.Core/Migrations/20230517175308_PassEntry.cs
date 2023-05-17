using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VipFit.Core.Migrations
{
    /// <inheritdoc />
    public partial class PassEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pass_Client_ClientId",
                table: "Pass");

            migrationBuilder.DropForeignKey(
                name: "FK_Pass_PassTemplate_PassTemplateId",
                table: "Pass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pass",
                table: "Pass");

            migrationBuilder.DropColumn(
                name: "IsActive",
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

            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PositionInPass = table.Column<byte>(type: "INTEGER", nullable: false),
                    PassId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entry_Passes_PassId",
                        column: x => x.PassId,
                        principalTable: "Passes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entry_PassId",
                table: "Entry",
                column: "PassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passes_Client_ClientId",
                table: "Passes",
                column: "ClientId",
                principalTable: "Client",
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
                name: "FK_Passes_Client_ClientId",
                table: "Passes");

            migrationBuilder.DropForeignKey(
                name: "FK_Passes_PassTemplate_PassTemplateId",
                table: "Passes");

            migrationBuilder.DropTable(
                name: "Entry");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Pass",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pass",
                table: "Pass",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pass_Client_ClientId",
                table: "Pass",
                column: "ClientId",
                principalTable: "Client",
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
