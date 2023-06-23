using Vega.Api.Data.Entities.Base;
using Vega.Api.Interfaces.Entities.Base;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface IDataService : IVegaService
{
    List<TEntity> GetData<TEntity>() where TEntity : IBaseEntity;
}
