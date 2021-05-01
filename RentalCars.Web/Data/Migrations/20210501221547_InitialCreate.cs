using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RentalCars.Web.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Mileage = table.Column<float>(type: "REAL", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Mileage = table.Column<float>(type: "REAL", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BookingNumber = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CarId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RentalReturnId = table.Column<Guid>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Returns_RentalReturnId",
                        column: x => x.RentalReturnId,
                        principalTable: "Returns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Category", "Mileage", "Model" },
                values: new object[] { new Guid("b692dedd-6d32-4cc6-b9e1-5cc90377418e"), "Compact", 0f, "Zaporozhets" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Category", "Mileage", "Model" },
                values: new object[] { new Guid("96e8e24c-2892-4a26-9643-6f45594d24e4"), "Premium", 0f, "Lamborghini Huracán" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Category", "Mileage", "Model" },
                values: new object[] { new Guid("9e542e08-0cdb-4e97-8706-0098d706cb14"), "Minivan", 0f, "Volvo" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Category", "Mileage", "Model" },
                values: new object[] { new Guid("18d3bc01-f34f-4b82-9e13-7e0c48aaa111"), "Premium", 0f, "Porsche Panamera" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Category", "Mileage", "Model" },
                values: new object[] { new Guid("12df1935-375d-4a05-a0b9-c2db5aa22841"), "Compact", 0f, "Lada Riva" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "DateOfBirth", "Email" },
                values: new object[] { new Guid("3b19a1ff-e68b-4929-ab8b-3c6f5ed094e4"), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test@testsson.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CarId",
                table: "Bookings",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RentalReturnId",
                table: "Bookings",
                column: "RentalReturnId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Returns");
        }
    }
}
