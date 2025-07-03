FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Product.API/Product.API.csproj", "Product.API/"]
COPY ["Product.Application/Product.Application.csproj", "Product.Application/"]
COPY ["Product.Infrastructure/Product.Infrastructure.csproj", "Product.Infrastructure/"]
COPY ["Product.Domain/Product.Domain.csproj", "Product.Domain/"]

RUN dotnet restore "Product.API/Product.API.csproj"

COPY . .

WORKDIR "/src/Product.API"
RUN dotnet build "Product.API.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM build AS publish
RUN dotnet publish "Product.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.API.dll"]
