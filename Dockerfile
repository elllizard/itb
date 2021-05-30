FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY ./itb/*.csproj ./
RUN dotnet restore

COPY ./itb ./
RUN dotnet publish -c Release -o Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/Release ./

CMD ASPNETCORE_URLS=http://*:$PORT dotnet itb.dll