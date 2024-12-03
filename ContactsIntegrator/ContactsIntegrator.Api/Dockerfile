# Use the base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the .csproj file from the correct location in the repository
COPY ["ContactsIntegrator/ContactsIntegrator.Api/ContactsIntegrator.Api.csproj", "ContactsIntegrator.Api/"]
RUN dotnet restore "ContactsIntegrator.Api/ContactsIntegrator.Api.csproj"

# Copy the full project source
COPY ["ContactsIntegrator", "/src/ContactsIntegrator"]
WORKDIR "/src/ContactsIntegrator.Api"
RUN dotnet build "ContactsIntegrator.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContactsIntegrator.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactsIntegrator.Api.dll"]
