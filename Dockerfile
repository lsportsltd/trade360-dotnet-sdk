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

# Run tests
RUN dotnet test -c Release --no-restore --collect:"XPlat Code Coverage" --results-directory ./coverage

# Codacy Config
ENV CODACY_API_TOKEN=${CODACY_TOKEN}
ENV CODACY_ORGANIZATION_PROVIDER=gh
ENV CODACY_USERNAME=lsportsltd
ENV CODACY_PROJECT_NAME=trade360-dotnet-sdk

# Copy the coverage file to a known location
RUN find ./coverage -name 'coverage.cobertura.xml' -exec cp {} ./coverage/coverage.cobertura.xml \;

# Download and run the Codacy reporter
RUN curl -Ls https://coverage.codacy.com/get.sh -o codacy-coverage-reporter.sh && \
    bash codacy-coverage-reporter.sh report -l CSharp -r ./coverage/coverage.cobertura.xml