using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Names;
using Vega.Framework.Map;

namespace Vega.Engine.Services;

[VegaService(10)]
public class WorldService : BaseVegaService<WorldService>, IWorldService
{
    private readonly ITileService _tileService;
    private readonly INameService _nameService;
    public Point WorldMapSize { get; } = new(180, 180);
    public Point GridMapSize { get; } = new(250, 250);
    public WorldMap CurrentWorldMap { get; set; }
    public GridMap CurrentGridMap { get; set; }

    public WorldService(
        ILogger<WorldService> logger, IMessageBusService messageBusService, ITileService tileService,
        INameService nameService
    ) : base(
        logger,
        messageBusService
    )
    {
        _tileService = tileService;
        _nameService = nameService;
    }

    public Task<WorldMap> GenerateWorldMapAsync()
    {
        Logger.LogInformation("Generating world map...");
        var worldMap = new WorldMap(WorldMapSize.X, WorldMapSize.Y)
        {
            Name = _nameService.RandomName(NameTypeEnum.World)
        };


        Logger.LogInformation("World map generated.");
        return Task.FromResult<WorldMap>(null);
    }
}
