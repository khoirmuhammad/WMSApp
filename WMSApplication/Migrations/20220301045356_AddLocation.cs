using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WMSApplication.Migrations
{
    public partial class AddLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Floor = table.Column<byte>(type: "tinyint", nullable: false),
                    RackAisle = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false),
                    Pos = table.Column<byte>(type: "tinyint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    MaximumPallet = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Code);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
