using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWorks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingdataforDifficultiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("388d17c6-eda2-4e8f-a3b8-3912289396db"), "Easy" },
                    { new Guid("551ea2fe-4297-4363-945f-2903e67593e6"), "Hard" },
                    { new Guid("5699a4f3-1f0d-4146-b30f-77c21c338ee5"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("1c2f9436-3011-43ea-82f5-9dd0b541aa18"), "AKL", "Auckland", "www.imgakl.com" },
                    { new Guid("9e7b4d39-0659-4a16-861e-c0755e8ffbc0"), "JHB", "Johannesburg", "www.imgjhb.com" },
                    { new Guid("a7515634-ace7-4c87-9d7f-962509d7661b"), "CPT", "Cape Town", "www.imgcpt.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("388d17c6-eda2-4e8f-a3b8-3912289396db"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("551ea2fe-4297-4363-945f-2903e67593e6"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("5699a4f3-1f0d-4146-b30f-77c21c338ee5"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("1c2f9436-3011-43ea-82f5-9dd0b541aa18"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("9e7b4d39-0659-4a16-861e-c0755e8ffbc0"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("a7515634-ace7-4c87-9d7f-962509d7661b"));
        }
    }
}
