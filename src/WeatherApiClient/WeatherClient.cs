using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherApiClient.Contracts;

namespace WeatherApiClient
{
    public class WeatherClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private readonly string _apiKey;

        public WeatherClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<PwsObservationsResponse> GetPwsObservations(string stationId, Units units, CancellationToken cancellationToken = default)
        {
            var url = $"https://api.weather.com/v2/pws/observations/current?stationId={stationId}&format=json&units={units.ToQueryParameterValue()}&apiKey={_apiKey}";

            using (var response = await _httpClient.GetAsync(url, cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Unable to get observations: {response.ReasonPhrase}");

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<PwsObservationsResponse>(json);
            }
        }
    }
}
