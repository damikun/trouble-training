
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS sdk
WORKDIR /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

COPY /bin/Debug/net6.0/publish  ./
# COPY --from=sdk /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
ENTRYPOINT ["dotnet", "APIServer.dll"]