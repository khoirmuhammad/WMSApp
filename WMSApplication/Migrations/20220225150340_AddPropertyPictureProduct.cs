using Microsoft.EntityFrameworkCore.Migrations;

namespace WMSApplication.Migrations
{
    public partial class AddPropertyPictureProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureExtension",
                table: "Product",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "Product",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PictureExtension",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "Product");
        }
    }
}
