#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
ARG TOKEN
ENV TOKEN=${TOKEN}
RUN dotnet restore
RUN dotnet build --no-restore --configuration Release

# ---- Test & Coverage Stage ----
FROM build AS test
RUN dotnet test --no-build --configuration Release --collect:"XPlat Code Coverage" --results-directory ./coverage
# Move the coverage file to a known location
RUN find ./coverage -type f -name 'coverage.cobertura.xml' -exec cp {} ./coverage/coverage.cobertura.xml \; || true

# (Optional) Coverage upload to Codacy (uncomment if you want to upload in Docker build)
ARG CODACY_TOKEN
ENV CODACY_API_TOKEN=${CODACY_TOKEN}
ENV CODACY_ORGANIZATION_PROVIDER=gh
ENV CODACY_USERNAME=lsportsltd
ENV CODACY_PROJECT_NAME=trade360-dotnet-sdk
RUN /bin/bash -c "bash <(curl -Ls https://coverage.codacy.com/get.sh) report -l CSharp $(find . -name 'coverage.cobertura.xml' -printf '-r %p ') --commit-uuid $(git rev-parse HEAD)"


# ---- Publish Stage ----
FROM build AS publish
RUN dotnet publish --no-restore --configuration Release -o /app/publish

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "trade360-dotnet-sdk.Application.dll"]

