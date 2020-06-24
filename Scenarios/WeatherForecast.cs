using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scenarios
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public readonly static WeatherForecast[] staticSampleDataPage1;
        public readonly static WeatherForecast[] staticSampleDataPage2;
        private static string[] sampleSummaries = new[] { "Balmy", "Chilly", "Freezing", "Bracing" };

        private static WeatherForecast CreateSampleDataItem(int index) => new WeatherForecast
        {
            Date = DateTime.Now.Date.AddDays(index),
            Summary = sampleSummaries[index % sampleSummaries.Length],
            TemperatureC = index,
        };

        static WeatherForecast()
        {
            staticSampleDataPage1 = Enumerable.Range(0, 200).Select(CreateSampleDataItem).ToArray();
            staticSampleDataPage2 = Enumerable.Range(200, 200).Select(CreateSampleDataItem).ToArray();
        }
    }
}
