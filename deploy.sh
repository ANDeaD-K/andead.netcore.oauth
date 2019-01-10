#!/bin/bash
set -ev

docker build -t andead/netcore.oauth:latest publish/.

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker push andead/netcore.oauth:latest