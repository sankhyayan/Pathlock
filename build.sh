#!/usr/bin/env bash
# Build script for Render

set -o errexit

# Install .NET 9 SDK if not already installed
if ! command -v dotnet &> /dev/null; then
    echo "Installing .NET 9 SDK..."
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 9.0 --install-dir /opt/render/.dotnet
    export PATH="/opt/render/.dotnet:$PATH"
    export DOTNET_ROOT="/opt/render/.dotnet"
fi

cd backend
dotnet restore
dotnet publish -c Release -o out
