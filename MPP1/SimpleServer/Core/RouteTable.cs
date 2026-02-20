using System.Collections.Concurrent;
using SimpleServer.Http;
using SimpleServer.Exceptions;

namespace SimpleServer.Core;

public class RouteTable
{
    private readonly ConcurrentDictionary<string, Func<HttpRequest, Task<HttpResponse>>> _routes
        = new();

    public void Add(string path, Func<HttpRequest, Task<HttpResponse>> handler)
    {
        if (!_routes.TryAdd(path, handler))
            throw new RouteAlreadyExistsException(path);
    }

    public bool TryGet(string path,
        out Func<HttpRequest, Task<HttpResponse>> handler)
    {
        return _routes.TryGetValue(path, out handler!);
    }

    public int Count => _routes.Count;
}