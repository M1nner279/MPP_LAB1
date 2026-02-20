using System.Reflection;
using TestLib.exceptions;
using TestLib.attributes;

if (args.Length == 0)
{
    Console.WriteLine("Path to test assembly is required.");
    return;
}

var assemblyPath = args[0];

if (!File.Exists(assemblyPath))
{
    Console.WriteLine($"Assembly not found: {assemblyPath}");
    return;
}

var assembly = Assembly.LoadFrom(assemblyPath);

int passed = 0;
int failed = 0;

var testClasses = assembly.GetTypes()
    .Where(t => t.GetCustomAttribute<TestClassAttribute>() != null);

foreach (var testClass in testClasses)
{
    var instance = Activator.CreateInstance(testClass);

    var methods = testClass.GetMethods()
        .Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null);

    foreach (var method in methods)
    {
        ExecuteTest(instance, method, null);
    }
}

Console.WriteLine();
Console.WriteLine($"Total: {passed + failed}, Passed: {passed}, Failed: {failed}");

void ExecuteTest(object? instance, MethodInfo method, object[]? parameters)
{
    try
    {
        method.Invoke(instance, parameters);
        Console.WriteLine($"PASS: {method.DeclaringType?.Name}.{method.Name}");
        passed++;
    }
    catch (TargetInvocationException ex)
    {
        if (ex.InnerException is TestFailedException tf)
        {
            Console.WriteLine($"FAIL: {method.Name} -> {tf.Message}");
        }
        else
        {
            Console.WriteLine($"ERROR: {method.Name} -> {ex.InnerException?.Message}");
        }

        failed++;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {method.Name} -> {ex.Message}");
        failed++;
    }
}