using SadConsole;
using Vega.Api.Interfaces.Entities;
using Vega.Api.Map.GameObjects;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface ITileService : IVegaService, IVegaReloadableService
{
    string SelectedTileSetId { get; }


    (ColoredGlyph, bool isTransparent, bool isWalkable) GetTile<TTile>(TTile entity) where TTile : IHasTileEntity;

    BaseTerrainGameObject GetTerrainFromTileId<TTerrain>() where TTerrain : IHasTileEntity, new();

    ColoredGlyph GetGlyphFromHasTileEntity<TEntity>(TEntity entity) where TEntity : IHasTileEntity;
}
