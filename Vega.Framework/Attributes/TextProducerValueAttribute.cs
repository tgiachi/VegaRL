namespace Vega.Framework.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class TextProducerValueAttribute : Attribute
{
    public string Name { get; set; }

    public TextProducerValueAttribute(string name)
    {
        Name = name;
    }
}
