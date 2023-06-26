using SadConsole;
using Vega.Api.Interfaces.Entities;
using Vega.Api.Map.GameObjects.Terrain;
using Vega.Api.Map.GameObjects.Terrain.Base;
using Vega.Api.Map.GameObjects.Vegetation;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface ITileService : IVegaService, IVegaReloadableService
{
    string SelectedTileSetId { get; }


    (ColoredGlyph coloredGlyph, bool isTransparent, bool isWalkable) GetTile<TTile>(TTile entity) where TTile : IHasTile;

    TerrainGameObject CreateTerrainFromTileId(string terrainId);

    VegetationGameObject CreateVegetationFromTileId(string vegetationId);

    ColoredGlyph GetGlyphFromHasTileEntity<TEntity>(TEntity entity) where TEntity : IHasTile;
}
