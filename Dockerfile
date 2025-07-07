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

# Run tests with coverage collection
RUN dotnet test -c Release --no-restore --collect:"XPlat Code Coverage" --results-directory ./coverage

# Install reportgenerator tool for merging coverage reports
RUN dotnet tool install -g dotnet-reportgenerator-globaltool

# Merge all coverage reports into a single report
RUN /root/.dotnet/tools/reportgenerator \
    -reports:"./coverage/**/coverage.cobertura.xml" \
    -targetdir:"./coverage-merged" \
    -reporttypes:"Cobertura" \
    -assemblyfilters:"+*" \
    -classfilters:"+*" \
    -filefilters:"+*"

# Codacy Config
ENV CODACY_API_TOKEN=${CODACY_TOKEN}
ENV CODACY_ORGANIZATION_PROVIDER=gh
ENV CODACY_USERNAME=lsportsltd
ENV CODACY_PROJECT_NAME=trade360-dotnet-sdk

# Download and run the Codacy reporter with the merged coverage report
RUN curl -Ls https://coverage.codacy.com/get.sh -o codacy-coverage-reporter.sh && \
    bash codacy-coverage-reporter.sh report -l CSharp -r ./coverage-merged/Cobertura.xml