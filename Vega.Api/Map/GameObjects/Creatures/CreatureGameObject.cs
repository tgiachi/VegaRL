using SadConsole;
using SadRogue.Primitives;
using Vega.Api.Map.GameObjects.Base;

namespace Vega.Api.Map.GameObjects.Creatures;

public class CreatureGameObject : BaseGridGameObject
{
    public CreatureGameObject(
        string objectId, string symbol, ColoredGlyph appearance, Point position, bool isWalkable,
        bool isTransparent
    ) : base(objectId, symbol, appearance, position, (int)MapLayerType.Creatures, isWalkable, isTransparent)
    {

    }
}
