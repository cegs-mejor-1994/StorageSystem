using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationsShipsEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "MeasurementUnits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RawMaterialId",
                table: "MeasurementUnits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MeasurementUnitId",
                table: "InputInventories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "InputInventories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementUnits_CategoryId",
                table: "MeasurementUnits",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementUnits_RawMaterialId",
                table: "MeasurementUnits",
                column: "RawMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInventories_MeasurementUnitId",
                table: "InputInventories",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInventories_SupplierId",
                table: "InputInventories",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_InputInventories_MeasurementUnits_MeasurementUnitId",
                table: "InputInventories",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InputInventories_Suppliers_SupplierId",
                table: "InputInventories",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MeasurementUnits_Categories_CategoryId",
                table: "MeasurementUnits",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MeasurementUnits_RawMaterials_RawMaterialId",
                table: "MeasurementUnits",
                column: "RawMaterialId",
                principalTable: "RawMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputInventories_MeasurementUnits_MeasurementUnitId",
                table: "InputInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_InputInventories_Suppliers_SupplierId",
                table: "InputInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_MeasurementUnits_Categories_CategoryId",
                table: "MeasurementUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_MeasurementUnits_RawMaterials_RawMaterialId",
                table: "MeasurementUnits");

            migrationBuilder.DropIndex(
                name: "IX_MeasurementUnits_CategoryId",
                table: "MeasurementUnits");

            migrationBuilder.DropIndex(
                name: "IX_MeasurementUnits_RawMaterialId",
                table: "MeasurementUnits");

            migrationBuilder.DropIndex(
                name: "IX_InputInventories_MeasurementUnitId",
                table: "InputInventories");

            migrationBuilder.DropIndex(
                name: "IX_InputInventories_SupplierId",
                table: "InputInventories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MeasurementUnits");

            migrationBuilder.DropColumn(
                name: "RawMaterialId",
                table: "MeasurementUnits");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "InputInventories");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "InputInventories");
        }
    }
}
