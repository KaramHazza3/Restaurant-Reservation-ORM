using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FirstName", "LastName", "PhoneNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "alice@example.com", "Alice", "Smith", "111111", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "bob@example.com", "Bob", "Brown", "222222", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "charlie@example.com", "Charlie", "Davis", "333333", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "diana@example.com", "Diana", "Evans", "444444", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "ethan@example.com", "Ethan", "Foster", "555555", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Address", "CreatedAt", "CreatedBy", "Name", "OpeningHours", "PhoneNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "123 Main St", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0, "Korean BBQ", "10:00-22:00", "100001", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 2, "456 Elm St", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0, "Italian Bistro", "09:00-21:00", "100002", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 3, "789 Oak St", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0, "Sushi House", "11:00-23:00", "100003", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 4, "321 Pine St", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0, "Burger Place", "08:00-20:00", "100004", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 5, "654 Cedar St", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0, "Vegan Delight", "10:00-20:00", "100005", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 0 }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "FirstName", "LastName", "Position", "RestaurantId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "John", "Doe", "Chef", 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Jane", "Doe", "Manager", 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Tom", "Lee", "Waiter", 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Sara", "Kim", "Chef", 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Mike", "Wong", "Manager", 4, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "Name", "Price", "RestaurantId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Korean mixed rice", "Bibimbap", 12.5m, 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Marinated beef", "Bulgogi", 15m, 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Classic pasta", "Spaghetti", 10m, 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Fresh sushi", "Sushi Roll", 8.5m, 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Vegan salad", "Salad Bowl", 7m, 5, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CustomerId", "PartySize", "RestaurantId", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 0, 1, "Confirmed", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 0, 1, "Pending", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 0, 2, "Cancelled", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 0, 3, "Confirmed", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5, 0, 4, "Pending", new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "Id", "Capacity", "CreatedAt", "CreatedBy", "RestaurantId", "TableNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Discount", "EmployeeId", "ReservationId", "Status", "Tax", "TotalPrice", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, 1, 1, "Preparing", 0m, 40m, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, 2, 2, "Served", 0m, 30m, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, 3, 3, "Cancelled", 0m, 17m, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, 4, 4, "Preparing", 0m, 7m, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, 5, 5, "Served", 0m, 0m, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "ReservationTables",
                columns: new[] { "ReservationId", "TableId", "AssignedSeats", "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 1, 2, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, 1, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, 3, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, 4, 0, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "MenuItemId", "Notes", "OrderId", "Quantity", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, "No spice", 1, 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, "", 1, 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, "Extra cheese", 2, 3, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4, "No wasabi", 3, 2, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5, "Dressing on side", 4, 1, new DateTime(2025, 10, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ReservationTables",
                keyColumns: new[] { "ReservationId", "TableId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ReservationTables",
                keyColumns: new[] { "ReservationId", "TableId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "ReservationTables",
                keyColumns: new[] { "ReservationId", "TableId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "ReservationTables",
                keyColumns: new[] { "ReservationId", "TableId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "ReservationTables",
                keyColumns: new[] { "ReservationId", "TableId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");
        }
    }
}
