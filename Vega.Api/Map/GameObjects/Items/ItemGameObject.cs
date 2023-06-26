using SadConsole;
using SadRogue.Primitives;
using Vega.Api.Map.GameObjects.Base;

namespace Vega.Api.Map.GameObjects.Items;

public class ItemGameObject : BaseGridGameObject
{
    public ItemGameObject(
        string objectId, string symbol, ColoredGlyph appearance, Point position, bool isWalkable,
        bool isTransparent
    ) : base(objectId, symbol, appearance, position, (int)MapLayerType.Items, isWalkable, isTransparent)
    {
    }
}
