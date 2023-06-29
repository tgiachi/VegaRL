using GoRogue.MapGeneration;


namespace Vega.Framework.Generators;

public class RoadsAreaBuilder
{
    public static RoadsAreaBuilder Instance { get; } = new();

    public void GenerateBuildingArea(int width, int height)
    {
        var generator =
            new Generator(width, height).ConfigAndGenerateSafe(g => g.AddSteps(DefaultAlgorithms.BasicRandomRoomsMapSteps())).Generate();

    }
}
