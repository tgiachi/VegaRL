namespace Vega.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EntityDataAttribute : Attribute
{
    public string TypeName { get; set; }

    public EntityDataAttribute(string typeName)
    {
        TypeName = typeName;
    }
}
