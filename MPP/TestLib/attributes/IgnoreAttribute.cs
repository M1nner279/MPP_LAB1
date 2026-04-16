namespace TestLib.attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IgnoreAttribute : Attribute
{
    public string? Message { get; }

    public IgnoreAttribute() { }

    public IgnoreAttribute(string message)
    {
        Message = message;
    }
}