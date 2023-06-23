using System.Diagnostics;
using System.Reflection;
using DaysOfDarkness.Engine.Data.Directories;
using Humanizer;
using Microsoft.Extensions.Logging;
using SadRogue.Integration;
using Vega.Api.Attributes;
using Vega.Api.Data.Directories;
using Vega.Api.Data.Entities.Base;
using Vega.Api.Interfaces.Entities.Base;
using Vega.Api.Utils;
using Vega.Api.Utils.Json;
using Vega.Engine.Events;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(1)]
public class DataService : BaseVegaReloadableService<DataService>, IDataService
{
    private readonly IMessageBusService _messageBusService;
    private readonly DirectoriesConfig _directoriesConfig;

    private readonly Dictionary<string, Type> _dataTypes = new();
    private readonly Dictionary<Type, List<string>> _rawData = new();

    public DataService(
        ILogger<DataService> logger, IMessageBusService messageBusService, DirectoriesConfig directoriesConfig
    ) : base(logger)
    {
        _messageBusService = messageBusService;
        _directoriesConfig = directoriesConfig;
        ScanForDataTypes();
    }


    private void ScanForDataTypes()
    {
        AssemblyUtils.GetAttribute<EntityDataAttribute>()
            .ForEach(
                s =>
                {
                    var attribute = s.GetCustomAttribute<EntityDataAttribute>();
                    Logger.LogDebug("Found data type {Type} => {Name}", attribute.TypeName, s.Name);
                    _dataTypes.Add(attribute.TypeName, s);
                }
            );
    }

    private Task LoadJsonFile(string fileName)
    {
        var sw = Stopwatch.StartNew();
        var file = new FileInfo(fileName);

        Logger.LogInformation("Loading file {File} ({Size})", file.Name, file.Length.Bytes());
        var resultJson = JsonSerializerInstance.ParseDataType(File.ReadAllText(file.FullName), _dataTypes);

        foreach (var (key, value) in resultJson)
        {
            InsertRawData(_dataTypes[key], value);
        }

        Logger.LogInformation("File {File} loaded in {Elapsed}", file.Name, sw.Elapsed);

        return Task.CompletedTask;
    }

    private async Task LoadJsonFiles()
    {
        var sw = Stopwatch.StartNew();
        var files = Directory.GetFiles(_directoriesConfig[DirectoryNameType.Data], "*.json", SearchOption.AllDirectories);

        Logger.LogInformation("Loading {Count} files", files.Length);

        _messageBusService.Send(new LoadingDataEvent());
        foreach (var file in files)
        {
            try
            {
                await LoadJsonFile(file);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading file {File}: {Message}", file, ex.Message);
            }
        }

        Logger.LogInformation("Loaded {Count} files in {Elapsed}", files.Length, sw.Elapsed);
        _messageBusService.Send(new DataLoadedEvent());
    }

    private void InsertRawData(Type type, IEnumerable<string> data)
    {
        if (!_rawData.ContainsKey(type))
        {
            _rawData.Add(type, new List<string>());
        }

        _rawData[type].AddRange(data);
    }

    public override Task<bool> LoadAsync()
    {
        LoadJsonFiles();
        return Task.FromResult(true);
    }

    public List<TEntity> GetData<TEntity>() where TEntity : IBaseEntity
    {
        var type = typeof(TEntity);

        return !_rawData.ContainsKey(type)
            ? new List<TEntity>()
            : _rawData[type].Select(JsonSerializerInstance.Deserialize<TEntity>).ToList();
    }

    public override Task<bool> ReloadAsync()
    {
        _rawData.Clear();

        return Task.FromResult(true);
    }
}
