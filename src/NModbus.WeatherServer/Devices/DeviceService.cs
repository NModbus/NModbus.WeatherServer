using Microsoft.Extensions.Logging;
using NModbus;
using NModbus.Data;
using NModbus.Device;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherApiClient.Contracts;

namespace ModbusWeatherServer.Devices
{
    public abstract class DeviceService
    {
        private readonly IObservable<PwsObservationsResponse> _source;

        private ushort _readingNumber = 1;

        protected DeviceService(
            ILogger logger,
            IModbusFactory factory,
            ModbusConfiguration configuration,
            IObservable<PwsObservationsResponse> source)
        {
            Logger = logger;
            Factory = factory;
            Configuration = configuration;
            _source = source;
        }

        public abstract DeviceType DeviceType { get; }

        protected ILogger Logger { get; }

        protected IModbusFactory Factory { get; }

        protected ModbusConfiguration Configuration { get; }

        protected IModbusSlave Device { get; private set; }

        protected SlaveDataStore DataStore { get; private set; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            //Create the network
            var network = await CreateNetworkAsync();

            //Create a datastore
            DataStore = new SlaveDataStore();

            //Log the operations for debugging purposes.
            DataStore.HoldingRegisters.BeforeRead += (sender, args) => LogReadAction("HoldingRegister(s)", args);
            DataStore.InputRegisters.BeforeRead += (sender, args) => LogReadAction("InputRegister(s)", args);
            DataStore.CoilDiscretes.BeforeRead += (sender, args) => LogReadAction("CoilDiscrete(s)", args);
            DataStore.CoilInputs.BeforeRead += (sender, args) => LogReadAction("CoilInput(s)", args);

            DataStore.HoldingRegisters.BeforeWrite += (sender, args) => LogWriteAction("HoldingRegister(s)", args);
            DataStore.CoilDiscretes.BeforeWrite += (sender, args) => LogWriteAction("CoilInput(s)", args);

            Logger.LogInformation("Adding device with unit id: {UnitId}", Configuration.UnitId);

            Device = Factory.CreateSlave(Configuration.UnitId, DataStore);

            //Add the device to the network
            network.AddSlave(Device);

            using (_source.Subscribe(OnResponse))
            {
                //Listen
                await network.ListenAsync(cancellationToken);
            }
        }

        private void OnResponse(PwsObservationsResponse response)
        {
            try
            {
                var observation = response?.Observations.FirstOrDefault();
                var values = observation.Metric;

                if (values == null)
                {
                    Logger.LogWarning("No metric values were returned.");
                    return;
                }

                var registers = new ushort[]
                {
                    (ushort)(values.Temperature * 10.0),
                    (ushort)values.HeatIndex,
                    (ushort)values.DewPoint,
                    (ushort)values.WindChill,
                    (ushort)(values.WindGust ?? 0),
                    (ushort)(values.Pressure * 10.0),
                    (ushort)(values.PrecipitationRate ?? 0),
                    (ushort)values.PrecipitationTotal,
                    (ushort)values.Elevation,
                    _readingNumber++
                };

                DataStore.InputRegisters.WritePoints(0, registers);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Error setting registers.");
            }
        }

        private void LogReadAction(string type, PointEventArgs args)
        {
            Logger.LogInformation("{Count} {Type} read starting at {Address}.",
                args.NumberOfPoints,
                type,
                args.StartAddress);
        }

        private void LogWriteAction(string type, PointEventArgs<ushort> args)
        {
            var data = string.Join(", ", args.Points.Select(p => p.ToString()));

            Logger.LogInformation("{Count} {Type} written starting at {Address}: [{Data}]",
                args.NumberOfPoints,
                type,
                args.StartAddress,
                data);
        }

        private void LogWriteAction(string type, PointEventArgs<bool> args)
        {
            var data = string.Join(", ", args.Points.Select(p => p ? "1" : "0"));

            Logger.LogInformation("{Count} {Type} written starting at {Address}: [{Data}]",
                args.NumberOfPoints,
                type,
                args.StartAddress,
                data);
        }

        protected abstract Task<IModbusSlaveNetwork> CreateNetworkAsync();

        public abstract Task StopAsync();
    }

}