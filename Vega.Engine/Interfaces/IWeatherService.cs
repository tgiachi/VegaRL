using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface IWeatherService : IVegaService
{
    double Temperature { get; }
    double Humidity { get; }
    double WindSpeed { get; }
    double WindDirection { get; }
}
