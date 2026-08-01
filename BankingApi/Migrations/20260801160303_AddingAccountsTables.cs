using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddingAccountsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypesTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountName = table.Column<string>(type: "TEXT", nullable: false),
                    YearsOfDeposit = table.Column<int>(type: "INTEGER", nullable: false),
                    HasInterist = table.Column<bool>(type: "INTEGER", nullable: false),
                    IntrestRate = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxAccountLimit = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypesTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomersAccountsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateOfAccountActivation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartingBalance = table.Column<double>(type: "REAL", nullable: false),
                    CurrentBalance = table.Column<double>(type: "REAL", nullable: false),
                    AccountStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersAccountsTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomersAccountsTable_AccountTypesTable_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AccountTypesTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomersAccountsTable_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomersAccountsTable_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomersAccountsTable_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomersAccountsTable_CustomersTable_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomersTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomersAccountsTable_AccountId",
                table: "CustomersAccountsTable",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersAccountsTable_CreatedBy",
                table: "CustomersAccountsTable",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersAccountsTable_CustomerId",
                table: "CustomersAccountsTable",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersAccountsTable_DeletedBy",
                table: "CustomersAccountsTable",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersAccountsTable_UpdatedBy",
                table: "CustomersAccountsTable",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomersAccountsTable");

            migrationBuilder.DropTable(
                name: "AccountTypesTable");
        }
    }
}
