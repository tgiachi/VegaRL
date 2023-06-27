using Vega.Framework.Data;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Engine.Interfaces.Services;

public interface IVegaValidateService<in TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<DataValidationResult>> ValidateAsync(TEntity entity);
}
