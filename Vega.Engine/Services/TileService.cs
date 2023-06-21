using Microsoft.Extensions.Logging;
using SadConsole;
using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Tiles;
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
    private readonly Dictionary<string, TileEntity> _tiles = new();
    private readonly Dictionary<string, ColoredGlyph> _tileGlyphs = new();

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
        LoadTilesAsync();

        return Task.FromResult(true);
    }

    private Task LoadTilesAsync()
    {
        foreach (var tile in LoadData<TileEntity>())
        {
            _tiles.Add(tile.Id, tile);

            _tileGlyphs.Add(tile.Id, GetGlyphFromTileId(tile.Id));
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

    public ColoredGlyph GetGlyphFromTileId(string tileId)
    {
        if (_tileGlyphs.TryGetValue(tileId, out var cachedGlyph))
        {
            return cachedGlyph;
        }

        ColoredGlyph glyph;
        var backgroundColor = _colorService.GetColorByName(_tiles[tileId].Background);
        var foregroundColor = _colorService.GetColorByName(_tiles[tileId].Foreground);

        if (_tileSetMap.TryGetValue(tileId, out var map))
        {
            glyph = new ColoredGlyph(backgroundColor, foregroundColor, GetGlyphFromTileSet(map.Glyph));
        }
        else
        {
            glyph = new ColoredGlyph(backgroundColor, foregroundColor, GetGlyphFromTileSet(_tiles[tileId].Sym));
        }

        _tileGlyphs.Add(tileId, glyph);
        return glyph;
    }

    public TTerrain GetTerrainFromTileId<TTerrain>(string tileId) where TTerrain : BaseTerrainGameObject, new()
    {
        var glyph = GetGlyphFromTileId(tileId);
        return new TTerrain()
        {
            Appearance = {  Background = glyph.Background, Glyph = glyph.Glyph, Foreground = glyph.Foreground,},
            IsWalkable = _tiles[tileId].IsWalkable,
            IsTransparent = _tiles[tileId].IsTransparent
        };
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
