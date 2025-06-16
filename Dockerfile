FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TOKEN
WORKDIR /.
COPY . .
RUN dotnet restore --configfile ./nuget/nuget.config
RUN dotnet build -c Release --no-restore

FROM build as test
ARG CODACY_TOKEN

# Install Java for Codacy coverage reporter
RUN apt-get update && apt-get install -y default-jre

# Create the coverage directory
RUN mkdir -p coverage

# Run tests and generate coverage report to expected path
RUN dotnet test -c Release --no-restore \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=coverage/coverage.cobertura.xml

# Codacy Config
ENV CODACY_API_TOKEN=${CODACY_TOKEN}
ENV CODACY_ORGANIZATION_PROVIDER=gh
ENV CODACY_USERNAME=lsportsltd
ENV CODACY_PROJECT_NAME=trade360-dotnet-sdk

# Download and run the Codacy reporter
RUN curl -Ls https://coverage.codacy.com/get.sh | sh && \
    ./codacy-coverage-reporter report \
    -r coverage/coverage.cobertura.xml \
    --language CSharp
