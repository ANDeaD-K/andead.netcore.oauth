#!/bin/bash
set -ev

echo "dotnet andead.netcore.oauth.dll --client-id={$CLIENT_ID} --client-secret={$CLIENT_SECRET} --redirect-uri={$REDIRECT_URI} --issuer={$ISSUER} --audience={$AUDIENCE} --signing-key={$SIGNING_KEY}" >> ./publish/entrypoint.sh

docker build -t andead/netcore.oauth:latest publish/.

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker push andead/netcore.oauth:latest
