namespace Battleships.objects.Exceptions;

public class LocationUnavailableException : Exception
{
    public LocationUnavailableException()
    {
        
    }

    public LocationUnavailableException(string message) : base(message)
    {
        
    }

    public LocationUnavailableException(string message, Exception inner) : base(message, inner)
    {
        
    }
}