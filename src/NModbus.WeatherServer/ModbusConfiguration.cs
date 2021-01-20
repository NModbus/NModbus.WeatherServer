namespace ModbusWeatherServer
{
    public class ModbusConfiguration
    {
        /// <summary>
        /// The Modbus Unit Id. Defaults to 1.
        /// </summary>
        public byte UnitId { get; set; } = 1;

        /// <summary>
        /// The port on which to serve the Modbus data. Defaults to 502.
        /// </summary>
        public int Port { get; set; } = 502;

        /// <summary>
        /// The local IP Address to use for the Modbus server. This normally doesn't need to be specified.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// True to enable Modbus loggging.
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// The type of device (TCP or UDP). Defaults to TCP.
        /// </summary>
        public DeviceType DeviceType { get; set; } = DeviceType.Tcp;
    }
}
