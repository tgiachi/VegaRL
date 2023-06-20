namespace Vega.Api.Utils;

/// <summary>
///   This class is used to get all types in an assembly that implement a specific interface.
///   or have a specific attribute.
/// </summary>
public class AssemblyUtils
{
    /// <summary>
    ///  Gets all types in an assembly that implement a specific attribute.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public IEnumerable<Type> GetTypesWithAttribute<TAttribute>() where TAttribute : Attribute =>
        from assembly in AppDomain.CurrentDomain.GetAssemblies()
        from type in assembly.GetTypes()
        where type.IsDefined(typeof(TAttribute), true)
        select type;
}
