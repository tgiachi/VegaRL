namespace Vega.Framework.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ValidateEntityAttribute : Attribute
{
    public Type EntityType { get; set; }

    public ValidateEntityAttribute(Type entityType)
    {
        EntityType = entityType;
    }
}
