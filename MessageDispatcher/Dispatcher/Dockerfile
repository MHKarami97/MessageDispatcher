FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Dispatcher/Dispatcher.csproj", "Dispatcher/"]
RUN dotnet restore "Dispatcher/Dispatcher.csproj"
COPY . .
WORKDIR "/src/Dispatcher"
RUN dotnet build "Dispatcher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dispatcher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dispatcher.dll"]
