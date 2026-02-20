using SimpleServer.Http;

namespace SimpleServer.Core;

public class MiddlewarePipeline
{
    private readonly List<Func<HttpRequest, Func<Task<HttpResponse>>, Task<HttpResponse>>> _middlewares
        = new();

    public void Use(Func<HttpRequest, Func<Task<HttpResponse>>, Task<HttpResponse>> middleware)
    {
        _middlewares.Add(middleware);
    }

    public Task<HttpResponse> ExecuteAsync(
        HttpRequest request,
        Func<Task<HttpResponse>> finalHandler)
    {
        Func<Task<HttpResponse>> next = finalHandler;

        foreach (var middleware in _middlewares.AsEnumerable().Reverse())
        {
            var current = next;
            next = () => middleware(request, current);
        }

        return next();
    }
}