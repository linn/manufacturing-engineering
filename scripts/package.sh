#!/bin/bash
set -ev
source ./scripts/install.sh

# build the client app
cd ./src/Service.Host
npm ci
BUILD_ENV=production npm run build
cd ../..

# build dotnet application
dotnet publish
# dotnet publish ./src/Messaging.Host/ -c release
# dotnet publish ./src/Scheduling.Host/ -c release

# determine which branch this change is from
if [ -n "${GITHUB_HEAD_REF}" ]; then
  # GitHub Actions PR
  GIT_BRANCH=$GITHUB_HEAD_REF
elif [ -n "${GITHUB_REF_NAME}" ]; then
  # GitHub Actions push
  GIT_BRANCH=$GITHUB_REF_NAME
elif [ "${TRAVIS_PULL_REQUEST}" = "false" ]; then
  # Travis push
  GIT_BRANCH=$TRAVIS_BRANCH
else
  # Travis PR
  GIT_BRANCH=$TRAVIS_PULL_REQUEST_BRANCH
fi

# create docker image(s)
echo "DOCKER_HUB_USERNAME is: $DOCKER_HUB_USERNAME"
docker login -u $DOCKER_HUB_USERNAME -p $DOCKER_HUB_PASSWORD

# Use GitHub Actions build number if available, fallback to Travis
BUILD_NUMBER="${GITHUB_RUN_NUMBER:-${TRAVIS_BUILD_NUMBER}}"

docker build --no-cache -t linn/manufacturing-engineering:$BUILD_NUMBER --build-arg gitBranch=$GIT_BRANCH ./src/Service.Host/

# push to dockerhub 
docker push linn/manufacturing-engineering:$BUILD_NUMBER
