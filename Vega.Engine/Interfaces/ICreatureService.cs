using SadRogue.Primitives;
using Vega.Api.Map.GameObjects.Creatures;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface ICreatureService : IVegaService
{
    CreatureGameObject CreateCreatureGameObject(string creatureId, Point position);
}
