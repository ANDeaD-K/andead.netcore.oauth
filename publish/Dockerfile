FROM microsoft/dotnet:2.2-aspnetcore-runtime

ADD . /app
WORKDIR /app

ENTRYPOINT dotnet andead.netcore.oauth.dll --client-id=$CLIENT_ID --client-secret=$CLIENT_SECRET --redirect-uri=$REDIRECT_URI --issuer=$ISSUER --audience=$AUDIENCE --signing-key=$SIGNING_KEY