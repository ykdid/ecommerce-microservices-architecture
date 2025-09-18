FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Services/ProductService/Product.API/Product.API.csproj", "Services/ProductService/Product.API/"]
COPY ["Services/ProductService/Product.Application/Product.Application.csproj", "Services/ProductService/Product.Application/"]
COPY ["Services/ProductService/Product.Infrastructure/Product.Infrastructure.csproj", "Services/ProductService/Product.Infrastructure/"]
COPY ["Services/ProductService/Product.Domain/Product.Domain.csproj", "Services/ProductService/Product.Domain/"]

RUN dotnet restore "Services/ProductService/Product.API/Product.API.csproj"

COPY . .

WORKDIR "/src/Services/ProductService/Product.API"
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
