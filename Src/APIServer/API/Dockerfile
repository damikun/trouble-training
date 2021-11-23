
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS sdk
WORKDIR /app

# RUN dotnet dev-certs https -ep /.aspnet/https/APIServer.pfx 
# RUN dotnet dev-certs https -i 
# RUN dotnet dev-certs https -t

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

COPY /bin/Debug/net5.0 ./
# COPY --from=sdk /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
ENTRYPOINT ["dotnet", "APIServer.dll"]