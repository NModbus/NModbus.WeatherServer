using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using WeatherApiClient;
using WeatherApiClient.Contracts;

namespace ModbusWeatherServer
{
    public class WeatherWorker : IHostedService
    {
        private readonly ILogger _logger;
        private readonly WeatherConfiguration _configuration;
        private readonly Subject<PwsObservationsResponse> _subject;

        private CancellationTokenSource _runCts;
        private WeatherClient _client;
        private Task _runTask;


        public WeatherWorker(
            ILogger<WeatherWorker> logger,
            WeatherConfiguration configuration,
            Subject<PwsObservationsResponse> subject)
        {
            _logger = logger;
            _configuration = configuration;
            _subject = subject;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new WeatherClient(_configuration.ApiKey);

            _runCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _runTask = RunAsync(_runCts.Token);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _runCts?.Cancel();

            if (_runTask != null)
            {
                await _runTask;
            }
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            //Give the Modbus server time to spin up
            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

            var pollInterval = TimeSpan.FromSeconds(_configuration.PollIntervalSeconds);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _client.GetPwsObservations(_configuration.StationId, Units.Metric, cancellationToken);

                    _subject.OnNext(response);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error talking to the weather service.");
                }

                try
                {
                    await Task.Delay(pollInterval, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                }
            }
        }
    }
}
