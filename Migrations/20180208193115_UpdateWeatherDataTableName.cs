using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WeatherData.Migrations
{
    public partial class UpdateWeatherDataTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherData_Addresses_AddressId",
                table: "WeatherData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherData",
                table: "WeatherData");

            migrationBuilder.RenameTable(
                name: "WeatherData",
                newName: "WeatherDatas");

            migrationBuilder.RenameIndex(
                name: "IX_WeatherData_AddressId",
                table: "WeatherDatas",
                newName: "IX_WeatherDatas_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherDatas",
                table: "WeatherDatas",
                column: "WeatherDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherDatas_Addresses_AddressId",
                table: "WeatherDatas",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherDatas_Addresses_AddressId",
                table: "WeatherDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherDatas",
                table: "WeatherDatas");

            migrationBuilder.RenameTable(
                name: "WeatherDatas",
                newName: "WeatherData");

            migrationBuilder.RenameIndex(
                name: "IX_WeatherDatas_AddressId",
                table: "WeatherData",
                newName: "IX_WeatherData_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherData",
                table: "WeatherData",
                column: "WeatherDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherData_Addresses_AddressId",
                table: "WeatherData",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
