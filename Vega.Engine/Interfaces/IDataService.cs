using Vega.Engine.Interfaces.Services;
using Vega.Framework.Interfaces.Entities.Base;

namespace Vega.Engine.Interfaces;

public interface IDataService : IVegaService
{
    List<TEntity> GetData<TEntity>() where TEntity : IBaseEntity;
}
