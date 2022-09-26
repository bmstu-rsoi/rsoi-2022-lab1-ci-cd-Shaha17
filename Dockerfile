FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

EXPOSE 8080

WORKDIR /app

COPY publish/ .

ENTRYPOINT ["dotnet", "PersonService.API.dll"]