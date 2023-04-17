# https://github.com/Azure/azure-functions-docker/blob/dev/host/4/bullseye/amd64/node/node18/node18.Dockerfile
# https://github.com/Azure/azure-functions-docker
FROM mcr.microsoft.com/azure-functions/python:4-python3.10-core-tools

# Install Node.js  
ARG NODE_VERSION="18"  
RUN apt-get update && \
    apt-get install -y curl gnupg && \
    curl -sL https://deb.nodesource.com/setup_${NODE_VERSION}.x | bash - && \
    apt-get update && \
    apt-get install -y nodejs

RUN curl -Lo /usr/local/bin/bicep https://github.com/Azure/bicep/releases/latest/download/bicep-linux-x64 \
    && chmod +x /usr/local/bin/bicep 