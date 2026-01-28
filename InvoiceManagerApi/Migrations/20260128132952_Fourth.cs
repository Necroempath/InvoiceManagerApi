using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class Fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceRows_Invoices_InvoiceId",
                table: "InvoiceRows");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRows_Invoices_InvoiceId",
                table: "InvoiceRows",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceRows_Invoices_InvoiceId",
                table: "InvoiceRows");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRows_Invoices_InvoiceId",
                table: "InvoiceRows",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id");
        }
    }
}
