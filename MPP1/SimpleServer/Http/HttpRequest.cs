namespace SimpleServer.Http;

public class HttpRequest
{
    public string Method { get; }
    public string Path { get; }
    public string Body { get; }

    public HttpRequest(string method, string path, string body = "")
    {
        Method = method ?? throw new ArgumentNullException(nameof(method));
        Path = path ?? throw new ArgumentNullException(nameof(path));
        Body = body ?? string.Empty;
    }
}