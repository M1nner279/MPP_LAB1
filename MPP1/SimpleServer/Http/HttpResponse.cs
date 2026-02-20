namespace SimpleServer.Http;

public class HttpResponse
{
    public int StatusCode { get; set; }
    public string Body { get; set; } = "";
}