#!/bin/bash
set -ev

dotnet restore

export NVM_DIR="$HOME/.nvm"
{ [ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"; } >/dev/null 2>&1

nvm install 22 >/dev/null 2>&1
nvm use 22 >/dev/null 2>&1

echo "Using Node version: $(node -v)"
