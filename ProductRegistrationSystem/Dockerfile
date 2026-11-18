
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ProductRegistrationSystem/ProductRegistrationSystem.csproj ProductRegistrationSystem/
RUN dotnet restore ProductRegistrationSystem/ProductRegistrationSystem.csproj

COPY . .
RUN dotnet publish ProductRegistrationSystem/ProductRegistrationSystem.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "ProductRegistrationSystem.dll"]
