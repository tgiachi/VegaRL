using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace Vega.Api.Map.GameObjects.Base;

public class BaseGridGameObject : RogueLikeEntity
{
    public string ObjectId { get; set; }
    public string Symbol { get; set; }

    public string TileId { get; set; }

    public bool IsAnimated { get; set; } = false;

    public List<int> AnimationFrames { get; set; } = new List<int>();

    private int _currentAnimationFrame = 0;

    public TimeSpan AnimationSpeed { get; set; } = TimeSpan.FromMilliseconds(100);
    private TimeSpan _animationTimer = TimeSpan.Zero;

    public BaseGridGameObject(
        string objectId, string symbol, ColoredGlyph appearance,
        Point position, int layer, bool isWalkable, bool isTransparent
    ) : base(position, appearance, isWalkable, isTransparent, layer)
    {
        Position = position;
        ObjectId = objectId;
        Symbol = symbol;
    }

    public override void Update(TimeSpan delta)
    {
        if (IsAnimated)
        {
            if (AnimationFrames.Count > 0)
            {
                _animationTimer += delta;
                if (_animationTimer >= AnimationSpeed)
                {
                    _currentAnimationFrame++;
                    if (_currentAnimationFrame >= AnimationFrames.Count)
                    {
                        _currentAnimationFrame = 0;
                    }

                    Appearance.Glyph = AnimationFrames[_currentAnimationFrame];
                    _animationTimer = TimeSpan.Zero;
                }
            }
        }

        base.Update(delta);
    }

    public override string ToString() => $"ObjectId: {ObjectId} - Symbol: {Symbol} - Position: {Position}";
}
