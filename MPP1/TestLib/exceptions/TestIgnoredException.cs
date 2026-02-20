namespace TestLib.exceptions;

public class TestIgnoredException : Exception
{
    public TestIgnoredException(string message) : base(message) { }
}

public class InvalidTestSignatureException : Exception
{
    public InvalidTestSignatureException(string message) : base(message) { }
}