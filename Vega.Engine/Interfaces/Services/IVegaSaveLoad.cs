using Vega.Api.Data.Serializer;

namespace Vega.Engine.Interfaces.Services;

public interface IVegaSaveLoad<TEntity> where TEntity : SerializedObject<TEntity>
{
    Task<bool> LoadAsync(TEntity entity);
    Task<TEntity> SaveAsync();
}
