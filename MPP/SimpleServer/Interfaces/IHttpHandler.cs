using SimpleServer.Http;

namespace SimpleServer.Interfaces;

public interface IHttpHandler
{
    Task<HttpResponse> HandleAsync(HttpRequest request);
}