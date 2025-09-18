FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["Services/StockService/Stock.API/Stock.API.csproj", "Services/StockService/Stock.API/"]
COPY ["Services/StockService/Stock.Application/Stock.Application.csproj", "Services/StockService/Stock.Application/"]
COPY ["Services/StockService/Stock.Domain/Stock.Domain.csproj", "Services/StockService/Stock.Domain/"]
COPY ["Services/StockService/Stock.Infrastructure/Stock.Infrastructure.csproj", "Services/StockService/Stock.Infrastructure/"]
COPY ["BuildingBlocks/EventBus/EventBus.csproj", "BuildingBlocks/EventBus/"]

# Restore dependencies
RUN dotnet restore "Services/StockService/Stock.API/Stock.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/Services/StockService/Stock.API"
RUN dotnet build "Stock.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stock.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stock.API.dll"]