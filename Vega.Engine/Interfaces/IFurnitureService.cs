using System.Collections;
using SadRogue.Primitives;
using Vega.Engine.Interfaces.Services;
using Vega.Framework.Data.Entities.Furniture;
using Vega.Framework.Map.GameObjects.Furniture;

namespace Vega.Engine.Interfaces;

public interface IFurnitureService : IVegaService
{
    FurnitureEntity GetFurniture(string id);
    FurnitureGameObject CreateFurnitureGameObject(string id, Point position);

    IEnumerable<FurnitureEntity> GetFurnituresWithFlag(string flag);
}
