using SadConsole;
using SadRogue.Primitives;
using Vega.Api.Data.Entities.Stats;
using Vega.Api.Map.GameObjects.Base;

namespace Vega.Api.Map.GameObjects.Creatures;

public class CreatureGameObject : BaseGridGameObject
{

    public BaseStatEntity Stats { get; set; } = new();

    public CreatureGameObject(
        string objectId, string symbol, string name, string description, ColoredGlyph appearance, Point position,
        bool isWalkable,
        bool isTransparent
    ) : base(
        objectId,
        symbol,
        name,
        description,
        appearance,
        position,
        (int)MapLayerType.Creatures,
        isWalkable,
        isTransparent
    )
    {
    }
}
