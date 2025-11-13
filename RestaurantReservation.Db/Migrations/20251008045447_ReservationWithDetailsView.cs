using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    public partial class ReservationWithDetailsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW ReservationWithDetails AS
				SELECT 
					res.Id AS ReservationId,
					r.Id AS RestaurantId,
					r.Name AS RestaurantName,
					r.Address AS RestaurantAddress
					, c.Id AS CustomerId,
					CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName
					FROM Reservations res
					JOIN Customers c
					ON c.Id = res.CustomerId
					JOIN Restaurants r
					ON r.Id = res.RestaurantId;
            ");
        }
        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("DROP VIEW ReservationWithDetails");
        }
    }
}
