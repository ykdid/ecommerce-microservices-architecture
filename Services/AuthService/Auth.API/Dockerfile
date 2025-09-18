FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/AuthService/Auth.API/Auth.API.csproj", "Services/AuthService/Auth.API/"]
COPY ["Services/AuthService/Auth.Application/Auth.Application.csproj", "Services/AuthService/Auth.Application/"]
COPY ["Services/AuthService/Auth.Domain/Auth.Domain.csproj", "Services/AuthService/Auth.Domain/"]
COPY ["Services/AuthService/Auth.Infrastructure/Auth.Infrastructure.csproj", "Services/AuthService/Auth.Infrastructure/"]
RUN dotnet restore "Services/AuthService/Auth.API/Auth.API.csproj"
COPY . .
WORKDIR "/src/Services/AuthService/Auth.API"
RUN dotnet build "Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"]
