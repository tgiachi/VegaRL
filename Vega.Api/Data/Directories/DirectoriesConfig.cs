using Vega.Api.Data.Directories;
using Vega.Api.Utils;

namespace DaysOfDarkness.Engine.Data.Directories;

/// <summary>
///  Directories configuration.
/// </summary>
public class DirectoriesConfig
{
    public int Length => Directories.Count;
    public Dictionary<DirectoryNameType, string> Directories { get; }

    public DirectoriesConfig()
    {
        Directories = new Dictionary<DirectoryNameType, string>();
        foreach (var type in Enum.GetValues(typeof(DirectoryNameType)).Cast<DirectoryNameType>())
        {
            Directories.Add(type, type.ToString());
        }
    }

    public string this[DirectoryNameType index]
    {
        get => Directories[index];
        set => Directories[index] = value;
    }

    public void AddDirectory(DirectoryNameType type, string directory)
    {
        Directories.Add(type, directory);
    }

    public string GetDirectory(DirectoryNameType type) => Directories[type];

    public void Initialize(string rootDirectory)
    {
        Directory.CreateDirectory(rootDirectory);
        Directories[DirectoryNameType.Root] = rootDirectory;
        foreach (var directory in Directories.Where(s => s.Key != DirectoryNameType.Root))
        {
            Directories[directory.Key] = Path.Combine(rootDirectory, directory.Value.ToUnderscoreCase());
            Directory.CreateDirectory(Directories[directory.Key]);
        }
    }

    public override string ToString() => $" {nameof(Directories)}: {Directories} ";
}
