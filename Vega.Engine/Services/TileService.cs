using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SadConsole;
using Vega.Engine.Components.Terrain;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Terrain;
using Vega.Framework.Data.Entities.Tiles;
using Vega.Framework.Data.Entities.Vegetation;
using Vega.Framework.Interfaces.Entities;
using Vega.Framework.Map.GameObjects.Terrain;
using Vega.Framework.Map.GameObjects.Vegetation;
using Vega.Framework.Utils;
using Vega.Framework.Utils.Random;

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
    private readonly Dictionary<string, VegetationEntity> _vegetationEntities = new();


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
        LoadVegetationAsync();

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

    private Task LoadVegetationAsync()
    {
        foreach (var vegetation in LoadData<VegetationEntity>())
        {
            _vegetationEntities.Add(vegetation.Id.ToLower(), vegetation);
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


    public (ColoredGlyph coloredGlyph, bool isTransparent, bool isWalkable) GetTile<TTile>(TTile entity)
        where TTile : IHasTile
    {
        var coloredGlyph = GetGlyphFromHasTileEntity(entity);
        return (coloredGlyph, entity.IsTransparent, entity.IsWalkable);
    }


    public TerrainGameObject CreateTerrainFromTileId(string terrainId)
    {
        if (_terrainEntities.TryGetValue(terrainId.ToLower(), out var terrainEntity))
        {
            var coloredTile = GetTile(terrainEntity);
            var terrainGameObject = new TerrainGameObject(
                terrainEntity.Id,
                coloredTile.coloredGlyph,
                coloredTile.isTransparent,
                coloredTile.isWalkable
            );
            //TODO: Add component for dynamic color dark and light
            terrainGameObject.GoRogueComponents.Add(
                new TerrainDarkAppearanceComponent(_serviceProvider.GetRequiredService<IWorldService>())
            );

            return terrainGameObject;
        }

        throw new Exception($" Terrain with id {terrainId} not found");
    }

    public VegetationGameObject CreateVegetationFromTileId(string vegetationId)
    {
        if (_vegetationEntities.TryGetValue(vegetationId.ToLower(), out var vegetationEntity))
        {
            var coloredTile = GetTile(vegetationEntity);
            var vegetationGameObject = new VegetationGameObject(
                vegetationEntity.Id,
                coloredTile.coloredGlyph,
                coloredTile.isTransparent,
                coloredTile.isWalkable
            );
            //TODO: Add component for dynamic color dark and light
            vegetationGameObject.GoRogueComponents.Add(
                new TerrainDarkAppearanceComponent(_serviceProvider.GetRequiredService<IWorldService>())
            );

            return vegetationGameObject;
        }

        throw new Exception($" Vegetation with id {vegetationId} not found");
    }

    public VegetationGameObject CreateVegetationFromTileId<TTile>(TTile tile) where TTile : IHasTile, new()
    {
        var coloredTile = GetTile(tile);
        var vegetationGameObject = new VegetationGameObject(
            tile.Id,
            coloredTile.coloredGlyph,
            coloredTile.isWalkable,
            coloredTile.isTransparent
        );

        vegetationGameObject.GoRogueComponents.Add(
            new TerrainDarkAppearanceComponent(_serviceProvider.GetRequiredService<IWorldService>())
        );

        return vegetationGameObject;
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

    public IEnumerable<TerrainEntity> FindTerrainByFlags(params string[] flags) => _terrainEntities.Values.FindByFlags(flags);

    private static int GetGlyphFromTileSet(string glyph)
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
