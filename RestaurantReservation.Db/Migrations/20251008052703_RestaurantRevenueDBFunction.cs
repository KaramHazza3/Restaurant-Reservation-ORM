using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    public partial class RestaurantRevenueDBFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
			CREATE FUNCTION fn_CalculateRestaurantRevenue(@RestaurantId INT)
			RETURNS DECIMAL(10,2)
			AS
			BEGIN 
				DECLARE @Revenue DECIMAL(10,2);

				SELECT @Revenue = Sum(o.TotalPrice)
				FROM Orders o
				JOIN Reservations res
				ON o.ReservationId = res.Id
				WHERE res.RestaurantId = @RestaurantId;

				RETURN ISNULL(@Revenue, 0);
			END;
            ");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("DROP FUNCTION dbo.fn_CalculateRestaurantRevenue");
        }
    }
}
