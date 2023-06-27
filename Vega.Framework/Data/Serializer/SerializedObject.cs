namespace Vega.Framework.Data.Serializer;

public class SerializedObject<TEntity> where TEntity : class
{
    public string Type { get; set; }
    public TEntity Entity { get; set; }
}
