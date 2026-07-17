# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/Locatic/Locatic.csproj src/Locatic/
RUN dotnet restore src/Locatic/Locatic.csproj

COPY src/Locatic/ src/Locatic/
RUN dotnet publish src/Locatic/Locatic.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

RUN groupadd --system appgroup && useradd --system --gid appgroup appuser \
    && mkdir -p /data \
    && chown -R appuser:appgroup /data /app

USER appuser

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ConnectionStrings__DefaultConnection="Data Source=/data/locatic.db"
ENV DisableHttpsRedirect=true

EXPOSE 8080

ENTRYPOINT ["dotnet", "Locatic.dll"]
