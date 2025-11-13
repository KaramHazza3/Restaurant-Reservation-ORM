namespace RestaurantReservation.Helpers;

public static class ValidationHelper
{
    public static void EnsureNotNull(object obj, string paramName)
    {
        if (obj is null) throw new ArgumentNullException(paramName);
    }

    public static void EnsureRequiredFields(params (object Value, string Name)[] fields)
    {
        foreach (var (value, name) in fields)
        {
            if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
                throw new ArgumentException($"{name} must be provided.");
        }
    }
}