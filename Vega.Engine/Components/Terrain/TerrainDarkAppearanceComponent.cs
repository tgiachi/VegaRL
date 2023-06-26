using SadConsole;
using SadRogue.Integration.Components;
using Vega.Api.Map.GameObjects.Terrain;
using Vega.Engine.Interfaces;

namespace Vega.Engine.Components.Terrain;

public class TerrainDarkAppearanceComponent : RogueLikeComponentBase<TerrainGameObject>
{
    private readonly IWorldService _worldService;

    public TerrainDarkAppearanceComponent(IWorldService worldService) : base(true, true, false, false)
    {
    }

    public override void Update(IScreenObject host, TimeSpan delta)
    {
        // Parent?.
        base.Update(host, delta);
    }
}
