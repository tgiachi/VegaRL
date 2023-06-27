using SadConsole;
using SadRogue.Primitives;
using Vega.Framework.Map.GameObjects.Base;

namespace Vega.Framework.Map.GameObjects.Furniture;

public class FurnitureGameObject : BaseGridGameObject
{
    public FurnitureGameObject(
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
        (int)MapLayerType.Furniture,
        isWalkable,
        isTransparent
    )
    {
    }
}
