using System.Reflection;

namespace Vega.Framework.Utils;

public static class AssemblyUtils
{
    private static List<Assembly> Assemblies { get; } =
        new(AppDomain.CurrentDomain.GetAssemblies().ToList());

    public static Type? GetInterfaceOfType(Type type)
    {
        try
        {
            return type.GetInterfaces()[0];
        }
        catch
        {
            return null;
        }
    }

    public static List<Type>? GetInterfacesOfType(Type type)
    {
        try
        {
            return type.GetInterfaces().ToList();
        }
        catch
        {
            return null;
        }
    }

    public static void AddAssembly(Assembly assembly)
    {
        AddAssembly(assembly, false);
    }

    private static void AddAssembly(Assembly assembly, bool force)
    {
        if (Assemblies.FirstOrDefault(a => a == assembly) == null)
        {
            Assemblies.Add(assembly);
            return;
        }

        var existsAssembly = Assemblies.FirstOrDefault(a => a == assembly);
        if (force && existsAssembly != null)
        {
            Assemblies.Remove(existsAssembly);
            Assemblies.Add(assembly);
        }
    }

    /// <summary>
    ///     Check all App domain assembly.
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static Type? GetType(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null)
        {
            return type;
        }

        foreach (var a in Assemblies)
        {
            type = a.GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }

        return null;
    }

    public static List<Type> GetTypesImplementsInterface(Type customInterface)
    {
        var types = Assemblies
            .SelectMany(s => s.GetTypes())
            .Where(customInterface.IsAssignableFrom);

        return types.ToList();
    }

    /// <summary>
    ///   Check all types with custom attribute.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="attributeType"></param>
    /// <returns></returns>
    private static List<Type> GetTypesWithCustomAttribute(Assembly assembly, Type attributeType)
    {
        return assembly.GetTypes().Where(type => type.GetCustomAttributes(attributeType, true).Length > 0).ToList();
    }

    /// <summary>
    ///    Scan all assembly from attribute.
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="filter"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<Type> ScanAssembly(Type attribute, string filter = "*.dll", string? path = null)
    {
        var result = new List<Type>();

        // InitAppDomain();
        foreach (var assembly in Assemblies)
        {
            try
            {
                result.AddRange(GetTypesWithCustomAttribute(assembly, attribute));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        return result;
    }

    public static List<Type> GetAttribute<T>() =>
        // return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(T))).ToList();
        ScanAllAssembliesFromAttribute(typeof(T));

    /// <summary>
    ///    Take all assemblies from attribute type.
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public static List<Type> ScanAllAssembliesFromAttribute(Type attribute) => ScanAssembly(attribute);

    public static List<Assembly> GetAppAssemblies() => Assemblies;
}
