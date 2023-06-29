namespace Vega.Framework.Attributes.WorldGeneration;


[AttributeUsage(AttributeTargets.Class)]
public class WorldMapGenerationStrategyAttribute : Attribute
{
    public string Name { get; set; }
}
