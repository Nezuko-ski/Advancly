using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Advancly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rowversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "RowVersion",
                table: "Transactions",
                type: "bigint unsigned",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned",
                oldRowVersion: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "RowVersion",
                table: "Transactions",
                type: "tinyint unsigned",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldRowVersion: true);
        }
    }
}
