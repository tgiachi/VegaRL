using CommandLine;

namespace Vega.Framework.Data.Config;

public class VegaUiOption
{
    [Option('w', "width", Required = false, HelpText = "Screen width")]
    public int ScreenWidth { get; set; } = 80;

    [Option('h', "height", Required = false, HelpText = "Screen height")]
    public int ScreenHeight { get; set; } = 25;

    [Option('s', "scale", Required = false, HelpText = "Screen scale factor")]
    public int ScaleFactor { get; set; } = 2;

    public string DefaultColorScheme { get; set; } = "c_default";
    public string DefaultUiFont { get; set; } = "Assets/Fonts/IBM_ext.font";
    public string DefaultWorldMapFont { get; set; } = "Assets/Fonts/DefaultOverMap.font";
}
