using SimpleServer.Http;

namespace SimpleServer.Interfaces;

public interface IRouter
{
    void Register(string path, Func<HttpRequest, Task<HttpResponse>> handler);
    Task<HttpResponse> RouteAsync(HttpRequest request);
}