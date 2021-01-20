using System;
using System.Text.Json.Serialization;

namespace WeatherApiClient.Contracts
{
    public class PwsObservation
    {
        [JsonPropertyName("stationID")]
        public string StationId { get; set; }

        [JsonPropertyName("obsTimeUtc")]
        public string ObservedTimeUtc { get; set; }

        [JsonPropertyName("obsTimeLocal")]
        public string ObservedTimeLocal { get; set; }

        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonPropertyName("softwareType")]
        public string SoftwareType { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("solarRadiation")]
        public double? SolarRadiation { get; set; }

        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        [JsonPropertyName("realtimeFrequency")]
        public double? RealtimeFrequency { get; set; }

        [JsonPropertyName("epoch")]
        public int Epoch { get; set; }

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("uv")]
        public double? UV { get; set; }

        [JsonPropertyName("windir")]
        public double WindDirection { get; set; }

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }

        [JsonPropertyName("qcStatus")]
        public int QcStatus { get; set; }

        [JsonPropertyName("imperial")]
        public PwsReadings Imperial { get; set; }

        [JsonPropertyName("metric")]
        public PwsReadings Metric { get; set; }
    }
}
