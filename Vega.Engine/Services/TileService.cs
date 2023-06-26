using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SadConsole;
using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Terrain;
using Vega.Api.Data.Entities.Tiles;
using Vega.Api.Interfaces.Entities;
using Vega.Api.Map.GameObjects;
using Vega.Api.Map.GameObjects.Terrain;
using Vega.Api.Map.GameObjects.Terrain.Base;
using Vega.Api.Utils;
using Vega.Api.Utils.Random;
using Vega.Engine.Components.Terrain;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(4)]
public class TileService : BaseDataLoaderVegaService<TileService>, ITileService
{
    public string SelectedTileSetId { get; private set; }

    private readonly IServiceProvider _serviceProvider;

    private readonly IColorService _colorService;
    private readonly Dictionary<string, TileSetEntity> _tileSetEntities = new();
    private readonly Dictionary<string, ColoredGlyph> _tileGlyphsCache = new();
    private readonly Dictionary<string, TileMapEntity> _tileSetMap = new();
    private readonly Dictionary<string, TerrainEntity> _terrainEntities = new();


    public TileService(
        ILogger<TileService> logger, IDataService dataService, IColorService colorService,
        IMessageBusService messageBusService, IServiceProvider serviceProvider
    ) : base(
        logger,
        dataService,
        messageBusService
    )
    {
        _colorService = colorService;
        _serviceProvider = serviceProvider;
    }


    public override Task<bool> LoadAsync()
    {
        LoadTileSetsAsync();
        LoadTerrainAsync();

        return Task.FromResult(true);
    }

    private Task LoadTerrainAsync()
    {
        foreach (var terrain in LoadData<TerrainEntity>())
        {
            _terrainEntities.Add(terrain.Id, terrain);
        }

        return Task.CompletedTask;
    }


    private Task LoadTileSetsAsync()
    {
        foreach (var tileSet in LoadData<TileSetEntity>())
        {
            _tileSetEntities.Add(tileSet.Id, tileSet);
        }

        if (_tileSetEntities.Count > 0)
        {
            SelectedTileSetId = _tileSetEntities.FirstOrDefault().Key;
            foreach (var map in _tileSetEntities.FirstOrDefault().Value.Map)
            {
                _tileSetMap.Add(map.TileId, map);
            }
        }


        return Task.CompletedTask;
    }


    public (ColoredGlyph coloredGlyph, bool isTransparent, bool isWalkable) GetTile<TTile>(TTile entity) where TTile : IHasTile
    {
        var coloredGlyph = GetGlyphFromHasTileEntity(entity);
        return (coloredGlyph, entity.IsTransparent, entity.IsWalkable);
    }

    public BaseTerrainGameObject GetTerrainFromTileId<TTile>(TTile tile) where TTile : IHasTile, new()
    {
        var coloredTile = GetTile(tile);
        var terrainGameObject = new TerrainGameObject(tile.Id, coloredTile.Item1, coloredTile.Item2, coloredTile.Item3);

        //TODO: Add component for dynamic color dark and light
        terrainGameObject.GoRogueComponents.Add(
            new TerrainDarkAppearanceComponent(_serviceProvider.GetRequiredService<IWorldService>())
        );

        return terrainGameObject;
    }

    public ColoredGlyph GetGlyphFromHasTileEntity<TEntity>(TEntity entity) where TEntity : IHasTile
    {
        if (_tileSetMap.TryGetValue(entity.Id, out var tileMap))
        {
            if (_tileGlyphsCache.TryGetValue(tileMap.TileId + "_tileset", out var tileColoredGlyph))
            {
                return tileColoredGlyph;
            }

            var tiledGlyph = new ColoredGlyph(
                _colorService.GetColorByName(tileMap.Background),
                _colorService.GetColorByName(tileMap.Foreground),
                GetGlyphFromTileSet(tileMap.Glyph)
            );
            _tileGlyphsCache.Add(tileMap.TileId + "_tileset", tiledGlyph);
            return tiledGlyph;
        }


        if (entity.Sym != null)
        {
            if (_tileGlyphsCache.TryGetValue(entity.Id, out var symColoredGlyph))
            {
                return symColoredGlyph;
            }

            var scg = new ColoredGlyph(
                _colorService.GetColorByName(entity.Background),
                _colorService.GetColorByName(entity.Foreground),
                GetGlyphFromTileSet(entity.Sym)
            );
            _tileGlyphsCache.Add(entity.Id, scg);
            return scg;
        }

        throw new Exception("No tileId or sym found");
    }

    private int GetGlyphFromTileSet(string glyph)
    {
        if (int.TryParse(glyph, out int n))
        {
            return n;
        }

        if (glyph.StartsWith("~~"))
        {
            glyph = glyph.Replace("~~", "");
            var lowerGlyph = int.Parse(glyph.Split("-")[0]);
            var upperGlyph = int.Parse(glyph.Split("-")[1]);
            return RandomUtils.Range(lowerGlyph, upperGlyph);
        }

        return glyph[0];
    }

    public Task<bool> ReloadAsync() => Task.FromResult(true);
}
