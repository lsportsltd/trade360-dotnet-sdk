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
RUN dotnet restore --configfile ./nuget/NuGet.Config

# Build the project (Release)
RUN dotnet build --no-restore --configuration Release

# --- TEST STAGE ---
FROM build AS test
RUN dotnet test --no-build --configuration Release --collect:"XPlat Code Coverage" --results-directory ./coverage

# (Optional) Move/convert coverage to cobertura format if needed
# RUN reportgenerator -reports:./coverage/**/coverage.cobertura.xml -targetdir:./coverage

# --- COVERAGE UPLOAD (optional, if you want to upload in this stage) ---
# FROM test AS coverage
# RUN wget -qO - https://coverage.codacy.com/get.sh | bash -s -- report -l CSharp -r ./coverage/coverage.cobertura.xml

# Codacy environment variables
ENV CODACY_API_TOKEN=${CODACY_TOKEN}
ENV CODACY_ORGANIZATION_PROVIDER=gh
ENV CODACY_USERNAME=lsportsltd
ENV CODACY_PROJECT_NAME=${SERVICE_NAME}

# Send coverage report to Codacy
RUN /bin/bash -c "bash <(curl -Ls https://coverage.codacy.com/get.sh) report -l CSharp $(find . -name 'coverage.cobertura.xml' -printf '-r %p ') --commit-uuid $(git rev-parse HEAD)"

# Publish the application
RUN dotnet publish -c Release --no-restore -o /app/publish

# ---- Runtime Image ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /src/publish .

# Set entrypoint (update DLL name as needed)
ENTRYPOINT ["dotnet", "trade360-dotnet-sdk.Application.dll"]

