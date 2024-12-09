# Base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file to the build context
COPY ["TestSecApplication1.csproj", "./"]

# Restore dependencies
RUN dotnet restore "./TestSecApplication1.csproj"

# Copy the rest of the application files
COPY . .

# Build the application
RUN dotnet build "./TestSecApplication1.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TestSecApplication1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestSecApplication1.dll"]
