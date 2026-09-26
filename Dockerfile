FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["DoacaoWorker.Worker/DoacaoWorker.Worker.csproj", "DoacaoWorker.Worker/"]
COPY ["DoacaoWorker.Application/DoacaoWorker.Application.csproj", "DoacaoWorker.Application/"]
COPY ["DoacaoWorker.Domain/DoacaoWorker.Domain.csproj", "DoacaoWorker.Domain/"]
COPY ["DoacaoWorker.Infrastructure/DoacaoWorker.Infrastructure.csproj", "DoacaoWorker.Infrastructure/"]
RUN dotnet restore "DoacaoWorker.Worker/DoacaoWorker.Worker.csproj"
COPY . .
WORKDIR "/src/DoacaoWorker.Worker"
RUN dotnet build "DoacaoWorker.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DoacaoWorker.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DoacaoWorker.Worker.dll"]
