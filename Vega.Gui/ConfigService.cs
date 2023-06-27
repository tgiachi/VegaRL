using System;
using System.IO;
using System.Text.Json;
using CommandLine;
using Vega.Framework.Data.Config;
using Vega.Framework.Utils.Json;

namespace Vega.Gui;

public class ConfigService
{
    private static ConfigService _instance;

    public static ConfigService Instance => _instance ??= new();

    public VegaEngineOption Initialize(string[] args)
    {

        var commandLine = Parser.Default.ParseArguments<VegaEngineOption>(args).Value;

        var rootDirectory = Environment.GetEnvironmentVariable("VEGARL_DATA_DIR") ??
                            Path.Join(Directory.GetCurrentDirectory(), "vega");
        var configPath = Path.Join(rootDirectory, "vegarl.json");


        if (!Directory.Exists(rootDirectory))
        {
            Directory.CreateDirectory(rootDirectory);
        }

        if (!File.Exists(configPath))
        {
            var initialConfig = new VegaEngineOption();
            var json = JsonSerializer.Serialize(initialConfig, JsonSerializerUtility.DefaultOptions);
            File.WriteAllText(configPath, json);
        }

        var options = JsonSerializer.Deserialize<VegaEngineOption>(
            File.ReadAllText(configPath),
            JsonSerializerUtility.DefaultOptions
        );

        options.RootDirectory = rootDirectory;

        return options;
    }
}
