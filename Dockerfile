#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

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

FROM build AS publish
WORKDIR /src/trade360-dotnet-sdk
RUN dotnet publish -c Release --no-restore -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# DATADOG Config

# Set environment variables so the Tracer can actually work and collect the traces from the app
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/app/datadog/linux-x64/Datadog.Trace.ClrProfiler.Native.so
ENV DD_DOTNET_TRACER_HOME=/app/datadog

# The following env var is to enable Trace - Logs correlation
ENV DD_LOGS_INJECTION=1

#This ensures the consumer span is correctly closed immediately after the message is consumed from the topic, and the metadata (such as partition and offset) is recorded correctly. 
#Spans created from Kafka messages using the SpanContextExtractor API are children of the producer span, and siblings of the consumer span.
ENV DD_TRACE_KAFKA_CREATE_CONSUMER_SCOPE_ENABLED=false

# Script to create the directory for the log files
RUN /app/datadog/createLogPath.sh

ENTRYPOINT ["dotnet", "trade360-dotnet-sdk.Application.dll"]
