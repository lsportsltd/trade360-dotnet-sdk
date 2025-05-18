#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# ---- Base .NET SDK ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /src

# Copy project files
COPY . .

# NuGet token (if needed for private feeds)
ARG TOKEN
ENV TOKEN=${TOKEN}

# Codacy args
ARG CODACY_TOKEN
ARG SERVICE_NAME=trade360-dotnet-sdk

# Restore dependencies
RUN dotnet restore --configfile ./nuget/nuget.config

# Build the project (Release)
RUN dotnet build -c Release --no-restore

# Test with coverage
RUN dotnet test -c Release --no-restore --collect:"XPlat Code Coverage" --results-directory ./coverage

# Move coverage file to known location for Codacy
RUN find ./coverage -name 'coverage.cobertura.xml' -exec cp {} ./coverage/coverage.cobertura.xml \;

# Codacy environment variables
ENV CODACY_API_TOKEN=${CODACY_TOKEN}
ENV CODACY_ORGANIZATION_PROVIDER=gh
ENV CODACY_USERNAME=lsportsltd
ENV CODACY_PROJECT_NAME=${SERVICE_NAME}

# Send coverage report to Codacy
RUN apt-get update && apt-get install -y --no-install-recommends wget bash && \
    wget -qO - https://coverage.codacy.com/get.sh | bash -s -- report -l CSharp -r ./coverage/coverage.cobertura.xml

# Publish the application
RUN dotnet publish -c Release --no-restore -o /app/publish

# ---- Runtime Image ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set entrypoint (update DLL name as needed)
ENTRYPOINT ["dotnet", "trade360-dotnet-sdk.Application.dll"]

