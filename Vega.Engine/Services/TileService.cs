using Microsoft.Extensions.Logging;
using SadConsole;
using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Tiles;
using Vega.Api.Interfaces.Entities;
using Vega.Api.Map.GameObjects;
using Vega.Api.Utils;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(4)]
public class TileService : BaseDataLoaderVegaService<TileService>, ITileService
{
    public string SelectedTileSetId { get; private set; }

    private readonly IColorService _colorService;
    private readonly Dictionary<string, TileSetEntity> _tileSetEntities = new();

    private readonly Dictionary<string, ColoredGlyph> _tileGlyphsCache = new();

    private readonly Dictionary<string, TileMapEntity> _tileSetMap = new();


    public TileService(ILogger<TileService> logger, IDataService dataService, IColorService colorService) : base(
        logger,
        dataService
    )
    {
        _colorService = colorService;
    }


    public override Task<bool> LoadAsync()
    {
        LoadTileSetsAsync();

        return Task.FromResult(true);
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


    public (ColoredGlyph, bool isTransparent, bool isWalkable) GetTile<TTile>(TTile entity) where TTile : IHasTileEntity
    {
        var coloredGlyph = GetGlyphFromHasTileEntity(entity);
        return (coloredGlyph, entity.IsTransparent, entity.IsWalkable);
    }

    public BaseTerrainGameObject GetTerrainFromTileId<TTerrain>() where TTerrain : IHasTileEntity, new()
    {
        return null;
    }

    public ColoredGlyph GetGlyphFromHasTileEntity<TEntity>(TEntity entity) where TEntity : IHasTileEntity
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
