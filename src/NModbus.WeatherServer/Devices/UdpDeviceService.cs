using Microsoft.Extensions.Logging;
using NModbus;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using WeatherApiClient.Contracts;

namespace ModbusWeatherServer.Devices
{
    public class UdpDeviceService : DeviceService
    {
        private UdpClient _udpClient;

        public UdpDeviceService(
            ILogger<UdpDeviceService> logger,
            IModbusFactory factory,
            ModbusConfiguration configuration,
            IObservable<PwsObservationsResponse> source)
            : base(logger, factory, configuration, source)
        {
        }

        public override DeviceType DeviceType => DeviceType.Udp;

        protected override Task<IModbusSlaveNetwork> CreateNetworkAsync()
        {
            Logger.LogInformation("Starting Modbus UDP network on port {Port}",
                Configuration.Port);

            _udpClient = new UdpClient(Configuration.Port);

            //_udpClient.Connect(address, Configuration.Port);

            return Task.FromResult(Factory.CreateSlaveNetwork(_udpClient));
        }

        public override Task StopAsync()
        {
            _udpClient.Dispose();

            return Task.CompletedTask;
        }
    }
}