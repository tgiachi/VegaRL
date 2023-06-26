using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(10)]
public class WeatherService : BaseVegaService<WeatherService>, IWeatherService
{
    public double Temperature { get; private set; }
    public double Humidity { get; private set; }
    public double WindSpeed { get; private set; }
    public double WindDirection { get; private set; }

    public WeatherService(ILogger<WeatherService> logger, IMessageBusService messageBusService) : base(logger, messageBusService)
    {
    }
}
