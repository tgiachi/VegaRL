using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Names;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(4)]
public class NameService : BaseDataLoaderVegaService<NameService>, INameService
{
    public Dictionary<(NameTypeEnum, GenderTypeEnum), List<string>> _names = new();

    public NameService(ILogger<NameService> logger, IDataService dataService) : base(logger, dataService)
    {
    }

    public override Task<bool> LoadAsync()
    {
        foreach (var names in LoadData<NameEntity>())
        {
            if (!_names.ContainsKey((names.NameType, names.Gender)))
            {
                _names.Add((names.NameType, names.Gender), new List<string>());
            }

            _names[(names.NameType, names.Gender)].AddRange(names.Names);
        }

        return Task.FromResult(true);
    }
}
