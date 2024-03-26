# Manufacturing Engineering
Data capture and analysis software for the Manufacturing Engineering team at Linn.

## Solution summary
* Service.Host project provides forms and reporting for existing data and code in the LinnApps Oracle/postgres/delete-as-appropriate database.
* Messaging.Host project provides services for consuming messages (using Rabbit MQ). Not deployed at the moment.
* Scheduling.Host project provides utilities for scheduling tasks. Not deployed at the moment.

## Component technologies
* The backend services are dotnet core C# apps with minimal third party dependencies.
* Service.Host uses the .NET minimal API web framework
* The javascript client app is built with React and managed with npm and webpack.
* Persistence is to an Oracle/postgres/delete-as-appropriate database via EF Core.
* Continuous deployment via Docker container to AWS ECS using Travis CI.
* Messaging.Host uses the RabbitMQ C# client to interact with Rabbit Messages Queues

## Local running and Testing
### C# service
* Restore nuget packages.
* Run C# tests as preferred.
* run or debug the Service.Host project to start the backend.

### Client
* `npm install` to install npm packages.
* `npm start` to run client locally on port 3000.
* `npm test` to run javascript tests.

### Routes
With the current configuration, all requests to app.linn.co.uk/manufacturing-engineering* will be sent to this app via traefik..
