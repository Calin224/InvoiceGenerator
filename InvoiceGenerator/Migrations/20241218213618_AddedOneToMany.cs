using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceGenerator.Migrations
{
    /// <inheritdoc />
    public partial class AddedOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PdfDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PdfDocuments_UserId",
                table: "PdfDocuments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PdfDocuments_Users_UserId",
                table: "PdfDocuments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PdfDocuments_Users_UserId",
                table: "PdfDocuments");

            migrationBuilder.DropIndex(
                name: "IX_PdfDocuments_UserId",
                table: "PdfDocuments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PdfDocuments");
        }
    }
}
