using System.Collections;
using SadRogue.Primitives;
using Vega.Api.Data.Entities.Furniture;
using Vega.Api.Map.GameObjects.Furniture;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface IFurnitureService : IVegaService
{
    FurnitureEntity GetFurniture(string id);
    FurnitureGameObject CreateFurnitureGameObject(string id, Point position);

    IEnumerable<FurnitureEntity> GetFurnituresWithFlag(string flag);
}
