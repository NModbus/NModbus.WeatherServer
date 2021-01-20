using System.Text.Json.Serialization;

namespace WeatherApiClient.Contracts
{
    public class PwsObservationsResponse
    {
        [JsonPropertyName("observations")]
        public PwsObservation[] Observations { get; set; }
    }
}
