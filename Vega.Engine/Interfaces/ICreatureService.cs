using SadRogue.Primitives;
using Vega.Engine.Interfaces.Services;
using Vega.Framework.Map.GameObjects.Creatures;

namespace Vega.Engine.Interfaces;

public interface ICreatureService : IVegaService
{
    CreatureGameObject CreateCreatureGameObject(string creatureId, Point position);
}
