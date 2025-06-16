FROM build as test
ARG CODACY_TOKEN

# Install Java, curl, and bash for Codacy
RUN apt-get update && apt-get install -y default-jre curl bash

# Create coverage output folder
RUN mkdir -p coverage

# Run tests with coverage
RUN dotnet test -c Release --no-restore \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=coverage/coverage.cobertura.xml

# Codacy Env Variables
ENV CODACY_API_TOKEN=${CODACY_TOKEN}
ENV CODACY_ORGANIZATION_PROVIDER=gh
ENV CODACY_USERNAME=lsportsltd
ENV CODACY_PROJECT_NAME=trade360-dotnet-sdk

# Download and run the Codacy coverage reporter using bash
RUN curl -Ls https://coverage.codacy.com/get.sh -o get_codacy.sh && \
    bash get_codacy.sh && \
    ./codacy-coverage-reporter report \
    -r coverage/coverage.cobertura.xml \
    --language CSharp
