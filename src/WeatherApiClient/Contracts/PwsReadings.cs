using System.Text.Json.Serialization;

namespace WeatherApiClient.Contracts
{
    public class PwsReadings
    {
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }

        [JsonPropertyName("index")]
        public double HeatIndex { get; set; }

        [JsonPropertyName("dewpt")]
        public double DewPoint { get; set; }

        [JsonPropertyName("windChill")]
        public double WindChill { get; set; }

        [JsonPropertyName("windGust")]
        public double? WindGust { get; set; }

        [JsonPropertyName("pressure")]
        public double Pressure { get; set; }

        [JsonPropertyName("precipRate")]
        public double? PrecipitationRate { get; set; }

        [JsonPropertyName("precipTotal")]
        public double PrecipitationTotal { get; set; }

        [JsonPropertyName("elev")]
        public double Elevation { get; set; }
    }
}
