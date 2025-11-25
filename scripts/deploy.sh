#!/bin/bash
set -ev

echo "Installing AWS CLI..."
curl -s "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
unzip -q awscliv2.zip
sudo ./aws/install >/dev/null 2>&1
echo "AWS CLI installed."

# deploy on aws
# Handle GitHub Actions environment variables
if [ -n "${GITHUB_REF_NAME}" ]; then
  # Running in GitHub Actions
  CURRENT_BRANCH="${GITHUB_REF_NAME}"
  if [ "${GITHUB_EVENT_NAME}" = "pull_request" ]; then
    IS_PULL_REQUEST="true"
  else
    IS_PULL_REQUEST="false"
  fi
else
  # Fallback to Travis variables
  CURRENT_BRANCH="${TRAVIS_BRANCH}"
  IS_PULL_REQUEST="${TRAVIS_PULL_REQUEST}"
fi

if [ "${CURRENT_BRANCH}" = "main" ] || [ "${GITHUB_BASE_REF}" = "main" ]; then
  if [ "${IS_PULL_REQUEST}" = "false" ]; then
    # main branch push - deploy to production
    echo deploy to production

    aws s3 cp s3://$S3_BUCKET_NAME/manufacturing-engineering/production.env ./secrets.env

    STACK_NAME=manufacturing-engineering
    APP_ROOT=http://app.linn.co.uk
    PROXY_ROOT=http://app.linn.co.uk
    ENV_SUFFIX=
  else
    # pull request to main - deploy to sys
    echo deploy to sys

    aws s3 cp s3://$S3_BUCKET_NAME/manufacturing-engineering/sys.env ./secrets.env

    STACK_NAME=manufacturing-engineering-sys
    APP_ROOT=http://app-sys.linn.co.uk
    PROXY_ROOT=http://app.linn.co.uk
    ENV_SUFFIX=-sys
  fi
else
  # not main - deploy to int if required
  echo do not deploy to int
fi

# load the secret variables but hide the output from the travis log
source ./secrets.env > /dev/null 2>&1

# deploy the service to amazon
# Use GitHub Actions build number if available, fallback to Travis
BUILD_NUMBER="${GITHUB_RUN_NUMBER:-${TRAVIS_BUILD_NUMBER}}"

aws cloudformation deploy --stack-name $STACK_NAME --template-file ./aws/application.yml --parameter-overrides dockerTag=$BUILD_NUMBER databaseHost=$DATABASE_HOST databaseName=$DATABASE_NAME databaseUserId=$DATABASE_USER_ID databasePassword=$DATABASE_PASSWORD rabbitServer=$RABBIT_SERVER rabbitPort=$RABBIT_PORT rabbitUsername=$RABBIT_USERNAME rabbitPassword=$RABBIT_PASSWORD appRoot=$APP_ROOT proxyRoot=$PROXY_ROOT authorityUri=$AUTHORITY_URI viewsRoot=$VIEWS_ROOT pdfServiceRoot=$PDF_SERVICE_ROOT cognitoHost=$COGNITO_HOST cognitoClientId=$COGNITO_CLIENT_ID cognitoDomainPrefix=$COGNITO_DOMAIN_PREFIX entraLogoutUri=$ENTRA_LOGOUT_URI environmentSuffix=$ENV_SUFFIX --capabilities=CAPABILITY_IAM

echo "deploy complete"
