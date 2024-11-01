# Use the base image for the ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["rocketfin.api/RocketFinApi/RocketFinApi.csproj", "rocketfin.api/"]
COPY ["rocketfin.api/UnitTests/UnitTests.csproj", "UnitTests/"]
COPY ["rocketfin.api/RocketFinApp/RocketFinApp.csproj", "RocketFinApp/"]
COPY ["rocketfin.api/RocketFinDomain/RocketFinDomain.csproj", "RocketFinDomain/"]
COPY ["rocketfin.api/RocketFinInfrastructure/RocketFinInfrastructure.csproj", "RocketFinInfrastructure/"]

# Restore dependencies for all projects
RUN dotnet restore "rocketfin.api/RocketFinApi/RocketFinApi.csproj"

# Copy the entire source code for all projects
COPY . .

# Build the application
RUN dotnet build "rocketfin.api/RocketFinApi/RocketFinApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "rocketfin.api/RocketFinApi/RocketFinApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RocketFinApi.dll"]
