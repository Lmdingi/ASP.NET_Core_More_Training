using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class TIN_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "Persons",
                type: "varchar(8)",
                nullable: true,
                defaultValue: "ABC12345",
                oldClrType: typeof(string),
                oldType: "varchar()",
                oldNullable: true,
                oldDefaultValue: "ABC12345");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaxIdentificationNumber",
                table: "Persons",
                type: "varchar()",
                nullable: true,
                defaultValue: "ABC12345",
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldNullable: true,
                oldDefaultValue: "ABC12345");
        }
    }
}
