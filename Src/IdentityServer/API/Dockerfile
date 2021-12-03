FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

COPY /bin/Debug/net6.0/publish ./
ENTRYPOINT ["dotnet", "APIServer.dll"]