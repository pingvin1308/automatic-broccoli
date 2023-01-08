using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomaticBroccoli.DataAccess.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AlterAttachmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_OpenLoops_OpenLoopId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_OpenLoopId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "OpenLoopId",
                table: "Attachments");

            migrationBuilder.CreateTable(
                name: "AttachmentOpenLoop",
                columns: table => new
                {
                    AttachmentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    OpenLoopsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentOpenLoop", x => new { x.AttachmentsId, x.OpenLoopsId });
                    table.ForeignKey(
                        name: "FK_AttachmentOpenLoop_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentOpenLoop_OpenLoops_OpenLoopsId",
                        column: x => x.OpenLoopsId,
                        principalTable: "OpenLoops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentOpenLoop_OpenLoopsId",
                table: "AttachmentOpenLoop",
                column: "OpenLoopsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentOpenLoop");

            migrationBuilder.AddColumn<Guid>(
                name: "OpenLoopId",
                table: "Attachments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_OpenLoopId",
                table: "Attachments",
                column: "OpenLoopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_OpenLoops_OpenLoopId",
                table: "Attachments",
                column: "OpenLoopId",
                principalTable: "OpenLoops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
