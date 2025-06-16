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

# Install unzip for extracting ReportGenerator
RUN apt-get update && apt-get install -y unzip

# Run tests
RUN dotnet test -c Release --no-restore --collect:"XPlat Code Coverage" --results-directory ./coverage

# Codacy Config
ENV CODACY_API_TOKEN=${CODACY_TOKEN}
ENV CODACY_ORGANIZATION_PROVIDER=gh
ENV CODACY_USERNAME=lsportsltd
ENV CODACY_PROJECT_NAME=trade360-dotnet-sdk

# Download ReportGenerator (standalone .NET Core app)
RUN curl -L -o reportgenerator.zip https://github.com/danielpalme/ReportGenerator/releases/download/v5.2.4/reportgenerator-netcoreapp3.0.zip \
    && unzip reportgenerator.zip -d /reportgenerator \
    && ls -lh /reportgenerator

# Merge all coverage.cobertura.xml files into one
RUN dotnet /reportgenerator/ReportGenerator.dll \
    -reports:coverage/*/coverage.cobertura.xml \
    -targetdir:coverage/merged \
    -reporttypes:Cobertura

# Copy merged file to expected location for Codacy
RUN cp coverage/merged/Cobertura.xml coverage/coverage.cobertura.xml

# Download and run the Codacy reporter
RUN curl -Ls https://coverage.codacy.com/get.sh -o codacy-coverage-reporter.sh && \
    bash codacy-coverage-reporter.sh report -l CSharp -r ./coverage/coverage.cobertura.xml