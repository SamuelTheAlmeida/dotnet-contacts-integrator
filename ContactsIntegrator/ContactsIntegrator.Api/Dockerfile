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

# Copy .csproj files for all referenced projects and the solution file
COPY ["ContactsIntegrator/ContactsIntegrator.Api/ContactsIntegrator.Api.csproj", "ContactsIntegrator.Api/"]
COPY ["ContactsIntegrator/ContactsIntegrator.Application/ContactsIntegrator.Application.csproj", "ContactsIntegrator.Application/"]
COPY ["ContactsIntegrator/ContactsIntegrator.Domain/ContactsIntegrator.Domain.csproj", "ContactsIntegrator.Domain/"]
COPY ["ContactsIntegrator/ContactsIntegrator.SDK.ContactsApi/ContactsIntegrator.SDK.ContactsApi.csproj", "ContactsIntegrator.SDK.ContactsApi/"]
COPY ["ContactsIntegrator/ContactsIntegrator.SDK.MailChimp/ContactsIntegrator.SDK.MailChimp.csproj", "ContactsIntegrator.SDK.MailChimp/"]
COPY ["ContactsIntegrator/ContactsIntegrator.sln", "ContactsIntegrator/"]

# Restore the solution
RUN dotnet restore "/src/ContactsIntegrator/ContactsIntegrator.sln"

# Copy the full project source
COPY ["ContactsIntegrator", "/src/ContactsIntegrator"]

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
