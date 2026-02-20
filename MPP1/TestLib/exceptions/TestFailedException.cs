namespace TestLib.exceptions;

public class TestFailedException : Exception
{
    public TestFailedException(string message) : base(message) { }
}