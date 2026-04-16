namespace SimpleServer.Exceptions;

public class RouteAlreadyExistsException : Exception
{
    public RouteAlreadyExistsException(string path)
        : base($"Route '{path}' already exists.")
    {
    }
}