using SadConsole;
using SadRogue.Primitives;
using Vega.Api.Data.Entities.Items;
using Vega.Api.Map.GameObjects.Base;

namespace Vega.Api.Map.GameObjects.Furniture;

public class FurnitureGameObject : BaseGridGameObject
{
    public List<ItemEntity> ContainerItems { get; set; } = new();

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
