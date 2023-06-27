using SadConsole;
using Vega.Engine.Interfaces.Services;
using Vega.Framework.Data.Entities.Terrain;
using Vega.Framework.Interfaces.Entities;
using Vega.Framework.Map.GameObjects.Terrain;
using Vega.Framework.Map.GameObjects.Vegetation;

namespace Vega.Engine.Interfaces;

public interface ITileService : IVegaService, IVegaReloadableService
{
    string SelectedTileSetId { get; }


    (ColoredGlyph coloredGlyph, bool isTransparent, bool isWalkable) GetTile<TTile>(TTile entity) where TTile : IHasTile;

    TerrainGameObject CreateTerrainFromTileId(string terrainId);

    VegetationGameObject CreateVegetationFromTileId(string vegetationId);

    ColoredGlyph GetGlyphFromHasTileEntity<TEntity>(TEntity entity) where TEntity : IHasTile;

    IEnumerable<TerrainEntity> FindTerrainByFlags(params string[] flags);
}
