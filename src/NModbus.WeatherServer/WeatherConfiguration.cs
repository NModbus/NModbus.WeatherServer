namespace ModbusWeatherServer
{
    public class WeatherConfiguration
    {
        /// <summary>
        /// The weather underground api key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// The number of seconds between poll attempts.
        /// </summary>
        public int PollIntervalSeconds { get; set; } = 60 * 5;

        /// <summary>
        /// The id of the PWS (personal weather station).
        /// </summary>
        public string StationId { get; set; }
    }
}
