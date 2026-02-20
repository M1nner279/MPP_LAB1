using TestLib;
using TestLib.attributes;
using SimpleServer;
using SimpleServer.Core;
using SimpleServer.Http;

namespace SimpleServer.Tests;

[TestClass]
public class DataRowTests
{
    [TestMethod]
    [DataRow("/a", 404)]
    [DataRow("/b", 404)]
    public async Task UnknownRoutes_Return404(string path, int expected)
    {
        var server = new SimpleHttpServer();

        var response = await server.RouteAsync(
            new HttpRequest("GET", path));

        Assert.AreEqual(expected, response.StatusCode);
        Assert.IsFalse(response.StatusCode == 200);
    }

    [TestMethod]
    [DataRow(1, IgnoreMessage = "Demonstration of ignored test")]
    public void IgnoredTest(int value)
    {
        Assert.IsTrue(value > 0);
    }
}