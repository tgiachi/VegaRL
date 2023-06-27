namespace Vega.Framework.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ActionExecutorAttribute : Attribute
{
    public string Action { get; set; }

    public ActionExecutorAttribute(string action)
    {
        Action = action;
    }
}
