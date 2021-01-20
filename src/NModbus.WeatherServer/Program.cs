using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModbusWeatherServer.Devices;
using NModbus;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WeatherApiClient.Contracts;

namespace ModbusWeatherServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.AddEnvironmentVariables();
                    configHost.AddJsonFile("appsettings.json", true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ModbusWorker>();
                    services.AddHostedService<WeatherWorker>();

                    services.AddSingleton<IModbusLogger, ModbusLogger>();
                    services.AddSingleton(serviceProvider =>
                    {
                        var config = serviceProvider.GetRequiredService<IConfiguration>();

                        var configuration = new ModbusConfiguration();

                        config.GetSection("Modbus").Bind(configuration);

                        return configuration;
                    });
                    services.AddSingleton(serviceProvider =>
                    {
                        var config = serviceProvider.GetRequiredService<IConfiguration>();

                        var configuration = new WeatherConfiguration();

                        config.GetSection("Weather").Bind(configuration);

                        return configuration;
                    });

                    services.AddSingleton<IModbusFactory>(serviceProvider =>
                    {

                        var deviceConfiguration = serviceProvider.GetRequiredService<ModbusConfiguration>();

                        if (deviceConfiguration.Verbose)
                        {
                            var modbusLogger = serviceProvider.GetRequiredService<IModbusLogger>();

                            return new ModbusFactory(logger: modbusLogger);
                        }

                        return new ModbusFactory();
                    });

                    //We're not supposed to use a subject for this. I'm being lazy.
                    var conditionsSubject = new Subject<PwsObservationsResponse>();

                    services.AddSingleton(conditionsSubject);
                    services.AddSingleton<IObservable<PwsObservationsResponse>>(conditionsSubject);

                    //Device Services
                    services.AddSingleton<DeviceService, TcpDeviceService>();
                    services.AddSingleton<DeviceService, UdpDeviceService>();
                });
        }
    }
}
