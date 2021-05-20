# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY scaler/*.csproj ./scaler/
RUN dotnet restore

# copy everything else and build app
COPY scaler/. ./scaler/
WORKDIR /source/scaler
RUN dotnet publish -c Release -o /app --no-cache

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
RUN mkdir output
ENTRYPOINT ["dotnet", "scaler.dll"]
