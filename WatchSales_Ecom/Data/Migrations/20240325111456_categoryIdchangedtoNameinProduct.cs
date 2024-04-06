using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchSales_Ecom.Data.Migrations
{
    /// <inheritdoc />
    public partial class categoryIdchangedtoNameinProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Product");
        }
    }
}
