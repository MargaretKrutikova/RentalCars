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
                    Mileage = table.Column<decimal>(type: "TEXT", nullable: false),
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
                values: new object[] { new Guid("452fcaca-2ea9-4cdf-b655-5ecfa29fb5b8"), "Compact", 0f, "Zaporozhets" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Category", "Mileage", "Model" },
                values: new object[] { new Guid("a43912b6-af24-4e2e-91af-37b6a0d8fdb6"), "Premium", 0f, "Lamborghini Huracán" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Category", "Mileage", "Model" },
                values: new object[] { new Guid("8a7db1ed-c450-44cc-93ce-49df69258862"), "Minivan", 0f, "Volvo" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Category", "Mileage", "Model" },
                values: new object[] { new Guid("dc552f72-2edf-45ff-9257-690158b12eaa"), "Premium", 0f, "Porsche Panamera" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Category", "Mileage", "Model" },
                values: new object[] { new Guid("a5ef600f-1dcb-47b9-be60-f35991dde2d6"), "Compact", 0f, "Lada Riva" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "DateOfBirth", "Email" },
                values: new object[] { new Guid("4c0363c5-e75c-474b-902e-733632603d5c"), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "DateOfBirth", "Email" },
                values: new object[] { new Guid("f1bdd878-fbcf-4ae4-a83c-b2d66fc5d947"), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test2" });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingNumber", "CarId", "CustomerId", "EndDate", "RentalReturnId", "StartDate" },
                values: new object[] { new Guid("4b12b511-0f4d-4356-870e-d40ff7dd9499"), "ABC12", new Guid("452fcaca-2ea9-4cdf-b655-5ecfa29fb5b8"), new Guid("4c0363c5-e75c-474b-902e-733632603d5c"), new DateTime(2021, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingNumber", "CarId", "CustomerId", "EndDate", "RentalReturnId", "StartDate" },
                values: new object[] { new Guid("31bb81bc-c9b6-4bb7-9c15-2a88c59ea437"), "ABC13", new Guid("452fcaca-2ea9-4cdf-b655-5ecfa29fb5b8"), new Guid("f1bdd878-fbcf-4ae4-a83c-b2d66fc5d947"), new DateTime(2021, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2021, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

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
