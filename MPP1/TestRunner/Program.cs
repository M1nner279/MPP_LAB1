using System.Reflection;

string assemblyPath = "../../../../SimpleServer.Tests/bin/Debug/net9.0/SimpleServer.Tests.dll";

if (!File.Exists(assemblyPath))
{
    Console.WriteLine($"Assembly not found: {assemblyPath}");
    return;
}

var assembly = Assembly.LoadFrom(assemblyPath);
var runner = new TestRunner.TestRunner();
runner.RunAsync(assembly);