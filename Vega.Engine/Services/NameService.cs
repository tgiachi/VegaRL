using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Names;
using Vega.Api.Utils.Random;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(4)]
public class NameService : BaseDataLoaderVegaService<NameService>, INameService
{
    private readonly Dictionary<(NameTypeEnum, GenderTypeEnum), List<string>> _names = new();

    public NameService(ILogger<NameService> logger, IDataService dataService, IMessageBusService messageBusService) : base(
        logger,
        dataService,
        messageBusService
    )
    {
    }

    public override Task<bool> LoadAsync()
    {
        foreach (var names in LoadData<NameEntity>())
        {
            if (!_names.ContainsKey((names.Usage, names.Gender)))
            {
                _names.Add((names.Usage, names.Gender), new List<string>());
            }

            _names[(names.Usage, names.Gender)].AddRange(names.Names);
        }

        return Task.FromResult(true);
    }

    public string RandomName(NameTypeEnum usage, GenderTypeEnum gender = GenderTypeEnum.None)
    {
        if (_names.TryGetValue((usage, gender), out var names))
        {
            return names.RandomElement();
        }

        throw new Exception($"Name not found with {usage} and {gender} ");
    }
}
