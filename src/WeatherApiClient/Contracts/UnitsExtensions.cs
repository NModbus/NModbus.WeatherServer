using System;

namespace WeatherApiClient.Contracts
{
    internal static class UnitsExtensions
    {
        public static string ToQueryParameterValue(this Units units)
        {
            switch (units)
            {
                case Units.Imperial:
                    return "e";

                case Units.Metric:
                    return "m";

                default:
                    throw new ArgumentOutOfRangeException(nameof(units), $"Unexpected value: '{units}'");
            }
        }
    }
}
