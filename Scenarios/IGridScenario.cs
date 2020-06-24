using Microsoft.AspNetCore.Components;

namespace Scenarios
{
    public interface IGridScenario : IComponent
    {
        WeatherForecast[] Forecasts { get; set; }
    }
}
