# Search Service

This services is responsible full text search.

# Docker build

To build the image run the following command
```shell
docker build -f src/Meteor.Search.Api/Dockerfile -t sgermonenko/meteor-search:{version} --build-arg NUGET_USER={USERNAME} --build-arg NUGET_PASSWORD={PASSWORD} .
```
where:
- {version} is microservice release version
- {USERNAME} is private nuget feed username
- {PASSWORD} is private nuget feed password
