# --------------------------------
# Build step
# --------------------------------

FROM mcr.microsoft.com/dotnet/sdk:5.0 as Build

WORKDIR /src

#Copy the source files
COPY src/ ./

#Build it!
RUN dotnet publish NModbus.WeatherServer/NModbus.WeatherServer.csproj -c Release -o /out

# --------------------------------
# Actual image
# --------------------------------
FROM mcr.microsoft.com/dotnet/runtime:5.0

COPY --from=build /out /app

ENTRYPOINT [ "dotnet", "/app/NModbus.WeatherServer.dll" ]