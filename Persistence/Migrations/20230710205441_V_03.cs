using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "ServerLogs");

            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "DetailsLogs");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "ServerLogs",
                newName: "InsertDateTime");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Posts",
                newName: "InsertDateTime");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "DetailsLogs",
                newName: "InsertDateTime");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ServerLogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DetailsLogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InsertDateTime",
                table: "ServerLogs",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "InsertDateTime",
                table: "Posts",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "InsertDateTime",
                table: "DetailsLogs",
                newName: "CreateDate");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ServerLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "ServerLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DetailsLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "DetailsLogs",
                type: "datetime2",
                nullable: true);
        }
    }
}
