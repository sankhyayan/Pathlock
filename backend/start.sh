#!/usr/bin/env bash
# Start script for Render

# Ensure .NET is in PATH
export PATH="/opt/render/.dotnet:/opt/render/.dotnet/tools:$PATH"
export DOTNET_ROOT="/opt/render/.dotnet"

# If dotnet is still not found, install it
if ! command -v dotnet &> /dev/null; then
    echo "Installing .NET 9 SDK..."
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 9.0 --install-dir /opt/render/.dotnet
fi

cd out
/opt/render/.dotnet/dotnet TaskManager.API.dll --urls "http://0.0.0.0:$PORT"
