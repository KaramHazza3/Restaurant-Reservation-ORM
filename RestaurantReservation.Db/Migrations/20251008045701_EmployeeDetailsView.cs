using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    
    public partial class EmployeeDetailsView : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
			CREATE VIEW EmployeeWithDetails AS
				SELECT e.Id AS EmployeeId,
				CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
				e.Position,
				r.Id AS RestaurantId,
				r.Name AS RestaurantName,
				r.Address AS RestaurantAddress
				FROM Employees e
				JOIN Restaurants r
				ON r.Id = e.RestaurantId;
            ");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW EmployeeWithDetails");
        }
    }
}
