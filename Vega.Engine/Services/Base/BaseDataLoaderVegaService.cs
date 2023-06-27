using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Framework.Interfaces.Entities.Base;

namespace Vega.Engine.Services.Base;

public abstract class BaseDataLoaderVegaService<TService> :BaseVegaService<TService>
{
    private readonly IDataService _dataService;
    public BaseDataLoaderVegaService(ILogger<TService> logger, IDataService dataService, IMessageBusService messageBusService) : base(logger, messageBusService)
    {
        _dataService = dataService;
    }

    protected List<TEntity> LoadData<TEntity>() where TEntity : IBaseEntity => _dataService.GetData<TEntity>();
}
