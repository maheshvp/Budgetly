# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files first (and Directory.Build.props) so restore is cached
# independently of source-code changes.
COPY Directory.Build.props ./
COPY src/Budgetly.Domain/Budgetly.Domain.csproj src/Budgetly.Domain/
COPY src/Budgetly.Application/Budgetly.Application.csproj src/Budgetly.Application/
COPY src/Budgetly.Infrastructure/Budgetly.Infrastructure.csproj src/Budgetly.Infrastructure/
COPY src/Budgetly.Api/Budgetly.Api.csproj src/Budgetly.Api/
RUN dotnet restore src/Budgetly.Api/Budgetly.Api.csproj

# Now copy the rest of the source and publish.
COPY src/ src/
RUN dotnet publish src/Budgetly.Api/Budgetly.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

# The base image defines APP_UID for a non-root user; run as that user.
USER $APP_UID

ENTRYPOINT ["dotnet", "Budgetly.Api.dll"]
