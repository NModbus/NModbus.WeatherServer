# Weather Modbus Server

Uses the API at weather.com to publish the status of a Personal Weather Station (PWS).

# Configuration
The application can be configured via:

- appsettings.json
- command line arguments
- environment variables

|Setting|Default|Description|
|---|---|---|
|WEATHER__APIKEY| |The API key for weather.com (from weather underground).|
|WEATHER__POLLINTERVALSECONDS |60*5|The number of seconds between calls to the service.|
|WEATHER__STATIONID | |The id of the PWS to query for.|
|MODBUS__UNITID |1| The Modbus Unit Id.|
|MODBUS__PORT|502 |The port on which to serve Modbus.|
|MODBUS__IPADDRESS | |The local IP Address to use for the Modbus server. Not normally needed.|
|MODBUS__VERBOSE | false | Set this to get more logging for Modbus. |
|MODBUS__DEVICE_TYPE | Tcp | (Tcp/Udp) The type of Modbus transport to use|


# Running

docker-compose.yaml

```yaml
version: "3.7"
  services:
    mongodb-edge:
     image: nmodbus/modbusweather
       restart: always
       ports:
         - 502:502
        environment:
          WEATHER__APIKEY: QQQQQQQQQQQ
          WEATHER__STATIONID: FFFFFFFFF
```

or command line:

```bash
docker run -e WEATHER__APIKEY=QQQQQQQQQ -e WEATHER__STATIONID=FFFFFFFFF nmodbus/modbusweather
```