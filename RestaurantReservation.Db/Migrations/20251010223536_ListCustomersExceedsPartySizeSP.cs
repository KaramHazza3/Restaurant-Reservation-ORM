using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class ListCustomersExceedsPartySizeSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
			CREATE PROCEDURE sp_ListCustomersExceedsPartySizeReservations
				@PartySize INT
			AS
			BEGIN
				SET NOCOUNT ON;
				SELECT c.Id, CONCAT(c.FirstName, ' ', c.LastName) AS Name, c.Email, res.PartySize
				FROM Reservations res
				JOIN Customers c
				ON res.CustomerId = c.Id
				WHERE res.PartySize >3;
			END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ListCustomersExceedsPartySizeReservations;");
        }
    }
}
