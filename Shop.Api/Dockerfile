FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Shop.Api/Shop.Api.csproj", "Shop.Api/"]
COPY ["Shop.Application/Shop.Application.csproj", "Shop.Application/"]
COPY ["Shop.Data/Shop.Data.csproj", "Shop.Data/"]
COPY ["Shop.Domain/Shop.Domain.csproj", "Shop.Domain/"]
RUN dotnet restore "Shop.Api/Shop.Api.csproj"
COPY . .
WORKDIR "/src/Shop.Api"
RUN dotnet build "Shop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Shop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shop.Api.dll"]
