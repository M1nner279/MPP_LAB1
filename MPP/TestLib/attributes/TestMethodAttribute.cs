namespace TestLib.attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class TestMethodAttribute : Attribute
{
    public string? DisplayName { get; set; }
    public int TimeoutMilliseconds { get; set; }
}