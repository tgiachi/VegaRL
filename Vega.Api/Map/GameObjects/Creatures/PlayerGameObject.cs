using SadConsole;
using SadRogue.Primitives;

namespace Vega.Api.Map.GameObjects.Creatures;

public class PlayerGameObject : CreatureGameObject
{
    public PlayerGameObject(
        string objectId, string symbol, string name, ColoredGlyph appearance, Point position,
        bool isWalkable, bool isTransparent
    ) : base(objectId, symbol, name, string.Empty, appearance, position, isWalkable, isTransparent)
    {
    }
}
