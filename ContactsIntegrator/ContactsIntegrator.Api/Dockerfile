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

# Copy the full ContactsIntegrator directory, including all the projects and the solution file
COPY ["ContactsIntegrator", "/src/ContactsIntegrator"]

# Restore the solution
RUN dotnet restore "/src/ContactsIntegrator/ContactsIntegrator.sln"

# Build the solution
RUN dotnet build "/src/ContactsIntegrator/ContactsIntegrator.sln" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "/src/ContactsIntegrator/ContactsIntegrator.sln" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactsIntegrator.Api.dll"]
