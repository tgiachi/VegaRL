using System.Text.Json.Serialization;
using CommandLine;

namespace Vega.Framework.Data.Config;

public class VegaEngineOption
{
    [JsonIgnore]
    public string RootDirectory { get; set; }
    public VegaUiOption Ui { get; set; } = new();

    [Option('l', "lang", Required = false, HelpText = "Default language to use")]
    public string DefaultLanguage { get; set; } = "en";

}
