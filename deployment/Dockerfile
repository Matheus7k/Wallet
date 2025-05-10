FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wallet.Api/Wallet.Api.csproj", "Wallet.Api/"]
COPY ["Wallet.Domain/Wallet.Domain.csproj", "Wallet.Domain/"]
COPY ["Wallet.Application/Wallet.Application.csproj", "Wallet.Application/"]
COPY ["Wallet.CrossCutting.Exception/Wallet.CrossCutting.Exception.csproj", "Wallet.CrossCutting.Exception/"]
COPY ["Wallet.CrossCutting.Configuration/Wallet.CrossCutting.Configuration.csproj", "Wallet.CrossCutting.Configuration/"]
COPY ["Wallet.Infrastructure.Data/Wallet.Infrastructure.Data.csproj", "Wallet.Infrastructure.Data/"]
RUN dotnet restore "Wallet.Api/Wallet.Api.csproj"
COPY . .
WORKDIR "/src/Wallet.Api"
RUN dotnet build "./Wallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.Api.dll"]
