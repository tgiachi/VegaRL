namespace Vega.Framework.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class KeybindingAttribute : Attribute
{
    public string[] Commands { get; set; }

    public KeybindingAttribute(params string[] commands)
    {
        Commands = commands;
    }
}
