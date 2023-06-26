namespace Vega.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ActionExecutorAttribute : Attribute
{
    public string Action { get; set; }

    public ActionExecutorAttribute(string action)
    {
        Action = action;
    }
}
