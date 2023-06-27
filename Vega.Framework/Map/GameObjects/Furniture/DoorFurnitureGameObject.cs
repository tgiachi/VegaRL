using SadConsole;
using SadRogue.Primitives;

namespace Vega.Framework.Map.GameObjects.Furniture;

public class DoorFurnitureGameObject : FurnitureGameObject
{
    public bool IsOpen { get; set; }
    public ColoredGlyph OpenTile { get; set; } = null!;
    public ColoredGlyph ClosedTile { get; set; } = null!;


    public DoorFurnitureGameObject(
        string objectId, string symbol, string name, string description, ColoredGlyph appearance, Point position,
        bool isWalkable, bool isTransparent
    ) : base(objectId, symbol, name, description, appearance, position, isWalkable, isTransparent)
    {
    }

    public bool Open()
    {
        if (IsOpen)
        {
            return false;
        }

        IsOpen = true;
        Appearance = OpenTile;
        IsWalkable = true;
        IsTransparent = true;

        return true;
    }

    public bool Close()
    {
        if (!IsOpen)
        {
            return false;
        }

        IsOpen = false;
        Appearance = ClosedTile;
        IsWalkable = false;
        IsTransparent = false;

        return true;
    }

    public void Toggle()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
