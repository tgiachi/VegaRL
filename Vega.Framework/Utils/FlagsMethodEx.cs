using Vega.Framework.Interfaces.Entities.Base;

namespace Vega.Framework.Utils;

public static class FlagsMethodEx
{
    public static IEnumerable<TEntity> FindByFlags<TEntity>(this IEnumerable<TEntity> entities, params string[] flags) where TEntity : IBaseEntity
    {
        return entities.Where(e => e.Flags.Any(flags.Contains));
    }
}
