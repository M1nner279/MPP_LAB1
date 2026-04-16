namespace TestLib.attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class DataRowAttribute : Attribute
{
    public object[] Values { get; }
    public string? IgnoreMessage { get; set; }
    
    public DataRowAttribute(params object[] values)
    {
        Values = values;
    }
}