using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using Vega.Api.Attributes;
using Vega.Api.Data.Config;
using Vega.Api.Data.Entities.ColorSchema;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(3)]
public class ColorService : BaseDataLoaderVegaService<ColorService>, IColorService
{
    private readonly VegaEngineOption _vegaEngineOption;
    private readonly Dictionary<string, ColorSchemaEntity> _colorSchemas = new();
    public Dictionary<string, Color> Colors { get; } = new();

    public ColorService(ILogger<ColorService> logger, IDataService dataService, VegaEngineOption vegaEngineOption, MessageBusService messageBusService) : base(logger, dataService, messageBusService)
    {
        _vegaEngineOption = vegaEngineOption;
    }

    public override Task<bool> LoadAsync()
    {
        var colors = LoadData<ColorSchemaEntity>();
        foreach (var color in colors)
        {
            _colorSchemas.Add(color.Id, color);
        }

        foreach (var color in _colorSchemas[_vegaEngineOption.Ui.DefaultColorScheme].GetColors())
        {
            Colors.Add(color.Key.ToLower(), color.Value);
        }


        return Task.FromResult(true);
    }

    public Task<bool> ReloadAsync()
    {
        _colorSchemas.Clear();
        Colors.Clear();

        return Task.FromResult(true);
    }

    public Color GetColorByName(string name)
    {
        if (Colors.TryGetValue(name.ToLower(), out var color))
        {
            return color;
        }

        throw new Exception($"Color {name} not found");
    }
}
