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

# Run tests
RUN dotnet test -c Release --no-restore --collect:"XPlat Code Coverage" --results-directory ./coverage

# Codacy Config
ENV CODACY_API_TOKEN=${CODACY_TOKEN}
ENV CODACY_ORGANIZATION_PROVIDER=gh
ENV CODACY_USERNAME=lsportsltd
ENV CODACY_PROJECT_NAME=trade360-dotnet-sdk

RUN /bin/bash -c "bash <(curl -Ls https://coverage.codacy.com/get.sh) report -l CSharp $(find . -name 'coverage.cobertura.xml' -printf '-r %p ')"