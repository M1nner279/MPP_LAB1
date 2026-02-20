using System.Collections;
using TestLib.exceptions;

namespace TestLib;

public static class Assert
{
    public static void AreEqual<T>(T expected, T actual)
    {
        if (!Equals(expected, actual))
            throw new TestFailedException(
                $"AreEqual failed. Expected: <{expected}>. Actual: <{actual}>.");
    }
    
    public static void AreNotEqual<T>(T notExpected, T actual)
    {
        if (Equals(notExpected, actual))
            throw new TestFailedException(
                $"AreNotEqual failed. Value: <{actual}>.");
    }
    
    public static void IsTrue(bool condition)
    {
        if (!condition)
            throw new TestFailedException("IsTrue failed.");
    }
    
    public static void IsFalse(bool condition)
    {
        if (condition)
            throw new TestFailedException("IsFalse failed.");
    }
    
    public static void IsNull(object? value)
    {
        if (value != null)
            throw new TestFailedException("IsNull failed.");
    }
    
    public static void IsNotNull(object? value)
    {
        if (value == null)
            throw new TestFailedException("IsNotNull failed.");
    }
    
    public static void Contains<T>(T expected, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            if (Equals(item, expected))
                return;
        }

        throw new TestFailedException(
            $"Contains failed. Item <{expected}> not found.");
    }
    
    public static void DoesNotContain<T>(T notExpected, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            if (Equals(item, notExpected))
                throw new TestFailedException(
                    $"DoesNotContain failed. Item <{notExpected}> found.");
        }
    }
    
    public static void IsEmpty(IEnumerable collection)
    {
        foreach (var _ in collection)
            throw new TestFailedException("IsEmpty failed.");
    }
    
    public static void IsNotEmpty(IEnumerable collection)
    {
        foreach (var _ in collection)
            return;

        throw new TestFailedException("IsNotEmpty failed.");
    }
    
    public static void AreSame(object expected, object actual)
    {
        if (!ReferenceEquals(expected, actual))
            throw new TestFailedException("AreSame failed.");
    }
    
    public static void AreNotSame(object expected, object actual)
    {
        if (ReferenceEquals(expected, actual))
            throw new TestFailedException("AreNotSame failed.");
    }
    
    public static void Throws<TException>(Action action)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }
        catch (Exception ex)
        {
            throw new TestFailedException(
                $"Throws failed. Expected {typeof(TException).Name}, got {ex.GetType().Name}");
        }

        throw new TestFailedException(
            $"Throws failed. No exception thrown. Expected {typeof(TException).Name}");
    }
    
    public static async Task ThrowsAsync<TException>(Func<Task> action)
        where TException : Exception
    {
        try
        {
            await action();
        }
        catch (TException)
        {
            return;
        }
        catch (Exception ex)
        {
            throw new TestFailedException(
                $"ThrowsAsync failed. Expected {typeof(TException).Name}, got {ex.GetType().Name}");
        }

        throw new TestFailedException(
            $"ThrowsAsync failed. No exception thrown. Expected {typeof(TException).Name}");
    }
}