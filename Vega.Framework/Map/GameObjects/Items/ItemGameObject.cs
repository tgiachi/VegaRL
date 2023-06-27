using SadConsole;
using SadRogue.Primitives;
using Vega.Framework.Map.GameObjects.Base;

namespace Vega.Framework.Map.GameObjects.Items;

public class ItemGameObject : BaseGridGameObject
{
    public ItemGameObject(
        string objectId, string symbol, string name, string description, ColoredGlyph appearance, Point position,
        bool isWalkable,
        bool isTransparent
    ) : base(objectId, symbol, name, description, appearance, position, (int)MapLayerType.Items, isWalkable, isTransparent)
    {
    }
}
