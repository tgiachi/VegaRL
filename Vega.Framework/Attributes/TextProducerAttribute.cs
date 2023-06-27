namespace Vega.Framework.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TextProducerAttribute : Attribute
{
    public string Suffix { get; set; }

    public TextProducerAttribute(string suffix)
    {
        Suffix = suffix;
    }
}
