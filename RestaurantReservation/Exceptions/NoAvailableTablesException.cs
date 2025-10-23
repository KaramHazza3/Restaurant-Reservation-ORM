namespace RestaurantReservation.Exceptions;

[Serializable]
public class NoAvailableTablesException : Exception
{
    public NoAvailableTablesException()
    {
    }

    public NoAvailableTablesException(string? message) : base(message)
    {
    }
}