namespace Vega.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class VegaServiceAttribute : Attribute
{
    public int Priority { get; set; }

    public VegaServiceAttribute(int priority = 10)
    {
        Priority = priority;
    }
}
