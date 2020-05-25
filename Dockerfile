FROM tirelx/net_core_3_1_ms_fonts_gdi:latest AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY . .
WORKDIR "/IntegrationService.Web"
RUN dotnet build  "IntegrationService.Web.csproj" -c Release --source "https://api.nuget.org/v3/index.json" -o /app

FROM build AS publish
RUN dotnet publish "IntegrationService.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
WORKDIR /app

ENTRYPOINT ["dotnet", "IntegrationService.Web.dll"]