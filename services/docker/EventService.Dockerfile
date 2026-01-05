FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY . .

WORKDIR /app/src/EventService/EventService.API
RUN ls -R /app
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build app/publish .
ENTRYPOINT ["dotnet", "EventService.API.dll"]

