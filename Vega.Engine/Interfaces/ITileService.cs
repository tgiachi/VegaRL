using SadConsole;
using Vega.Api.Interfaces.Entities;
using Vega.Api.Map.GameObjects;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface ITileService : IVegaService, IVegaReloadableService
{
    string SelectedTileSetId { get; }

    ColoredGlyph GetGlyphFromTileId(string tileId);

    TTerrain GetTerrainFromTileId<TTerrain>(string tileId) where TTerrain : BaseTerrainGameObject, new();

    ColoredGlyph GetGlyphFromHasTileEntity<TEntity>(TEntity entity) where TEntity : IHasTileEntity;
}
