using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using Vega.Api.Attributes;
using Vega.Api.Data.Entities.ColorSchema;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;


[VegaService(3)]
public class ColorService : BaseDataLoaderVegaService<ColorService>, IColorService
{
    private readonly Dictionary<string, ColorSchemaEntity> _colorSchemas = new();
    public Dictionary<string, Color> Colors { get; } = new();

    public ColorService(ILogger<ColorService> logger, IDataService dataService) : base(logger, dataService)
    {

    }

    public override Task<bool> LoadAsync()
    {
        var colors = LoadData<ColorSchemaEntity>();
        foreach (var color in colors)
        {
            _colorSchemas.Add(color.Name, color);
        }

        foreach (var color in  _colorSchemas.FirstOrDefault().Value.GetColors())
        {
            Colors.Add(color.Key, color.Value);
        }



        return Task.FromResult(true);
    }

    public Task<bool> ReloadAsync()
    {
        return Task.FromResult(true);
    }
}
