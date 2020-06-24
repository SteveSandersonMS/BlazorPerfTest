using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scenarios;

#pragma warning disable BL0005
namespace BlazorPerfTest
{
    class Program
    {
        const int cycles = 100;

        static async Task Main(string[] args)
        {
            var serviceProvider = CreateServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var renderer = new PerfTestRenderer(serviceProvider, loggerFactory);
            
            var fastGridFromBlankTime = await TestGridFromBlankAsync<Scenarios.FastGrid.Scenario>(renderer);
            Console.WriteLine($"FastGrid from blank time: {fastGridFromBlankTime:F1}ms");
            var fastGridUpdateTime = await TestGridSwapPagesAsync<Scenarios.FastGrid.Scenario>(renderer);
            Console.WriteLine($"FastGrid swap pages time: {fastGridUpdateTime:F1}ms");

            var complexTableFromBlankTime = await TestGridFromBlankAsync<Scenarios.ComplexTable.Scenario>(renderer);
            Console.WriteLine($"ComplexTable from blank time: {complexTableFromBlankTime:F1}ms");
            var complexTableUpdateTime = await TestGridSwapPagesAsync<Scenarios.ComplexTable.Scenario>(renderer);
            Console.WriteLine($"ComplexTable swap pages time: {complexTableUpdateTime:F1}ms");
        }

        private static async Task<double> TestGridFromBlankAsync<T>(PerfTestRenderer renderer) where T: IGridScenario, new()
        {
            var startTime = DateTime.Now;

            for (var i = 0; i < cycles; i++)
            {
                var scenario = new T();
                var componentId = renderer.AssignRootComponentId(scenario);
                scenario.Forecasts = WeatherForecast.staticSampleDataPage1;

                // Note that although we're 'awaiting' this, all the work we care about actually happens synchronously,
                // because the data is already loaded up-front
                await renderer.RenderRootComponentAsync(componentId);
            }

            var endTime = DateTime.Now;
            var durationPerCycle = endTime.Subtract(startTime).TotalMilliseconds / cycles;
            return durationPerCycle;
        }

        private static async Task<double> TestGridSwapPagesAsync<T>(PerfTestRenderer renderer) where T : IGridScenario, new()
        {
            var startTime = DateTime.Now;
            var scenario = new T();
            var componentId = renderer.AssignRootComponentId(scenario);
            scenario.Forecasts = WeatherForecast.staticSampleDataPage1;
            await renderer.RenderRootComponentAsync(componentId);

            for (var i = 0; i < cycles; i++)
            {
                scenario.Forecasts = i % 2 == 0 ? WeatherForecast.staticSampleDataPage2 : WeatherForecast.staticSampleDataPage1;
                await renderer.RenderRootComponentAsync(componentId);
            }

            var endTime = DateTime.Now;
            var durationPerCycle = endTime.Subtract(startTime).TotalMilliseconds / cycles;
            return durationPerCycle;
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            return serviceCollection.BuildServiceProvider();
        }
    }
}
#pragma warning restore BL0005
