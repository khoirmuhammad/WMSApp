using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WMSApplication.Migrations
{
    public partial class AddProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WholeUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LooseQty = table.Column<int>(type: "int", nullable: false),
                    WholeQty = table.Column<int>(type: "int", nullable: false),
                    AllocationType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoosePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WholePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryCode = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    UnitCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory_CategoryCode",
                        column: x => x.CategoryCode,
                        principalTable: "ProductCategory",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_Unit_UnitCode",
                        column: x => x.UnitCode,
                        principalTable: "Unit",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryCode",
                table: "Product",
                column: "CategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Product_UnitCode",
                table: "Product",
                column: "UnitCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
