using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Advancly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rowversionupdate : Migration
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
                defaultValue: 1ul,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldRowVersion: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "RowVersion",
                table: "Transactions",
                type: "bigint unsigned",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldRowVersion: true,
                oldDefaultValue: 1ul);
        }
    }
}
