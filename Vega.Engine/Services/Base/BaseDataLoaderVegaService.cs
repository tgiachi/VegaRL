using Microsoft.Extensions.Logging;
using Vega.Api.Data.Entities.Base;
using Vega.Engine.Interfaces;

namespace Vega.Engine.Services.Base;

public abstract class BaseDataLoaderVegaService<TService> :BaseVegaService<TService>
{
    private readonly IDataService _dataService;
    public BaseDataLoaderVegaService(ILogger<TService> logger, IDataService dataService) : base(logger)
    {
        _dataService = dataService;
    }

    protected List<TEntity> LoadData<TEntity>() where TEntity : BaseEntity => _dataService.GetData<TEntity>();
}
