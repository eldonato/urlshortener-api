FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY UrlShortener.sln .

RUN mkdir UrlShortener
COPY UrlShortener/*.csproj ./UrlShortener/

RUN dotnet restore UrlShortener.sln

COPY UrlShortener/. ./UrlShortener/

WORKDIR /src/UrlShortener

RUN dotnet publish UrlShortener.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "UrlShortener.dll"]