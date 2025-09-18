FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["Services/OrderService/Order.API/Order.API.csproj", "Services/OrderService/Order.API/"]
COPY ["Services/OrderService/Order.Application/Order.Application.csproj", "Services/OrderService/Order.Application/"]
COPY ["Services/OrderService/Order.Domain/Order.Domain.csproj", "Services/OrderService/Order.Domain/"]
COPY ["Services/OrderService/Order.Infrastructure/Order.Infrastructure.csproj", "Services/OrderService/Order.Infrastructure/"]
COPY ["BuildingBlocks/EventBus/EventBus.csproj", "BuildingBlocks/EventBus/"]

# Restore dependencies
RUN dotnet restore "Services/OrderService/Order.API/Order.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/Services/OrderService/Order.API"
RUN dotnet build "Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"]