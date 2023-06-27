namespace Vega.Framework.Attributes;

/// <summary>
///   Attribute to define the type name of serialized entity data.
///   Every entity data class must have this attribute and will be deserialized to property name "$type" in json.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EntityDataAttribute : Attribute
{
    public string TypeName { get; set; }

    public EntityDataAttribute(string typeName)
    {
        TypeName = typeName;
    }
}
