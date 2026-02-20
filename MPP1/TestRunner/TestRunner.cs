using System.Reflection;
using TestLib.attributes;
using TestLib.exceptions;

namespace TestRunner;

public class TestRunner
{
    private int _passed;
    private int _failed;
    private int _ignored;
    private int _errors;

    public async Task RunAsync(Assembly assembly)
    {
        object? sharedInstance = await InitializeSharedContext(assembly);

        var testClasses = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<TestClassAttribute>() != null);

        foreach (var testClass in testClasses)
        {
            await ExecuteTestClass(testClass, sharedInstance);
        }

        await CleanupSharedContext(sharedInstance);

        PrintSummary();
    }

    // -------------------- Shared Context --------------------

    private async Task<object?> InitializeSharedContext(Assembly assembly)
    {
        var sharedType = assembly.GetTypes()
            .FirstOrDefault(t => t.GetCustomAttribute<SharedContextAttribute>() != null);

        if (sharedType == null)
            return null;

        var instance = Activator.CreateInstance(sharedType);

        var initMethod = sharedType.GetMethods()
            .FirstOrDefault(m => m.GetCustomAttribute<SharedContextInitializeAttribute>() != null);

        if (initMethod != null)
            await InvokeMethod(instance, initMethod);

        return instance;
    }

    private async Task CleanupSharedContext(object? sharedInstance)
    {
        if (sharedInstance == null)
            return;

        var cleanupMethod = sharedInstance.GetType()
            .GetMethods()
            .FirstOrDefault(m => m.GetCustomAttribute<SharedContextCleanupAttribute>() != null);

        if (cleanupMethod != null)
            await InvokeMethod(sharedInstance, cleanupMethod);
    }

    // -------------------- Test Class --------------------

    private async Task ExecuteTestClass(Type testClass, object? sharedInstance)
    {
        var classInit = testClass.GetMethods()
            .FirstOrDefault(m => m.GetCustomAttribute<ClassInitializeAttribute>() != null);

        var classCleanup = testClass.GetMethods()
            .FirstOrDefault(m => m.GetCustomAttribute<ClassCleanupAttribute>() != null);

        object? classInstanceForInit = CreateInstance(testClass, sharedInstance);

        if (classInit != null)
            await InvokeMethod(classInstanceForInit, classInit);

        var testMethods = testClass.GetMethods()
            .Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null);

        foreach (var method in testMethods)
        {
            await ExecuteTestMethod(testClass, method, sharedInstance);
        }

        if (classCleanup != null)
            await InvokeMethod(classInstanceForInit, classCleanup);
    }

    // -------------------- Test Method --------------------

    private async Task ExecuteTestMethod(Type testClass, MethodInfo method, object? sharedInstance)
    {
        var dataRows = method.GetCustomAttributes<DataRowAttribute>().ToList();

        if (!dataRows.Any())
        {
            await ExecuteSingleTest(testClass, method, null, sharedInstance);
        }
        else
        {
            foreach (var row in dataRows)
            {
                if (!string.IsNullOrWhiteSpace(row.IgnoreMessage))
                {
                    _ignored++;
                    continue;
                }

                await ExecuteSingleTest(testClass, method, row.Values, sharedInstance);
            }
        }
    }

    private async Task ExecuteSingleTest(Type testClass, MethodInfo method, object[]? parameters, object? sharedInstance)
    {
        object? instance = CreateInstance(testClass, sharedInstance);

        var setup = testClass.GetMethods()
            .FirstOrDefault(m => m.GetCustomAttribute<SetUpAttribute>() != null);

        var teardown = testClass.GetMethods()
            .FirstOrDefault(m => m.GetCustomAttribute<TearDownAttribute>() != null);

        try
        {
            if (setup != null)
                await InvokeMethod(instance, setup);

            await InvokeMethod(instance, method, parameters);

            _passed++;
            Console.WriteLine($"PASS: {method.DeclaringType?.Name}.{method.Name}");
        }
        catch (TestFailedException ex)
        {
            _failed++;
            Console.WriteLine($"FAIL: {method.Name} -> {ex.Message}");
        }
        catch (TestIgnoredException ex)
        {
            _ignored++;
            Console.WriteLine($"IGNORED: {method.Name} -> {ex.Message}");
        }
        catch (Exception ex)
        {
            _errors++;
            Console.WriteLine($"ERROR: {method.Name} -> {ex.InnerException?.Message ?? ex.Message}");
        }
        finally
        {
            if (teardown != null)
                await InvokeMethod(instance, teardown);
        }
    }

    // -------------------- Helpers --------------------

    private object? CreateInstance(Type testClass, object? sharedInstance)
    {
        var constructor = testClass.GetConstructors().First();

        var parameters = constructor.GetParameters();

        if (sharedInstance != null &&
            parameters.Length == 1 &&
            parameters[0].ParameterType == sharedInstance.GetType())
        {
            return Activator.CreateInstance(testClass, sharedInstance);
        }

        return Activator.CreateInstance(testClass);
    }

    private async Task InvokeMethod(object? instance, MethodInfo method, object[]? parameters = null)
    {
        var result = method.Invoke(instance, parameters);

        if (result is Task task)
            await task;
    }

    private void PrintSummary()
    {
        Console.WriteLine();
        Console.WriteLine("------ SUMMARY ------");
        Console.WriteLine($"Passed:  {_passed}");
        Console.WriteLine($"Failed:  {_failed}");
        Console.WriteLine($"Ignored: {_ignored}");
        Console.WriteLine($"Errors:  {_errors}");
    }
}