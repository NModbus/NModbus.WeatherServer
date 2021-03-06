﻿using Microsoft.Extensions.Logging;
using NModbus;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using WeatherApiClient.Contracts;

namespace ModbusWeatherServer.Devices
{
    public class TcpDeviceService : DeviceService
    {
        private TcpListener _tcpListener;

        public override DeviceType DeviceType => DeviceType.Tcp;

        public TcpDeviceService(
            ILogger<TcpDeviceService> logger,
            IModbusFactory factory,
            ModbusConfiguration configuration,
            IObservable<PwsObservationsResponse> source)
            : base(logger, factory, configuration, source)
        {
        }

        protected IPAddress GetIpAddress()
        {
            if (string.IsNullOrWhiteSpace(Configuration.IpAddress))
            {
                return IPAddress.Any;
            }

            return IPAddress.Parse(Configuration.IpAddress);
        }

        protected override Task<IModbusSlaveNetwork> CreateNetworkAsync()
        {
            var address = GetIpAddress();

            Logger.LogInformation("Starting Modbus TCP network on {IPAddress}:{Port}",
                address,
                Configuration.Port);

            //Start up a listener
            _tcpListener = new TcpListener(address, Configuration.Port);
            _tcpListener.Start();

            return Task.FromResult(Factory.CreateSlaveNetwork(_tcpListener));
        }

        public override Task StopAsync()
        {
            _tcpListener?.Stop();

            return Task.CompletedTask;
        }
    }

}