using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddingCostomersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomersTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CustomerDateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CustomerNationalId = table.Column<double>(type: "REAL", nullable: false),
                    IsMale = table.Column<bool>(type: "INTEGER", nullable: false),
                    Grade = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerEmail = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerPhoenNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersTable", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomersTable");
        }
    }
}
