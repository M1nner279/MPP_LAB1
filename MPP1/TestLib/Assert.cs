using TestLib.exceptions;

namespace TestLib;

public static class Assert
{ 
    public static void AreNotEqual<T>(T expected, T actual) 
    {
        if (Equals(expected, actual))
        {
            throw new TestFailedException($"Assert.AreNotEqual failed. Actual: <{actual}>. Expected: <{expected}>.");
        }
    }
}