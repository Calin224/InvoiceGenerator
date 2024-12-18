using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceGenerator.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFilePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "PdfDocuments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "PdfDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
