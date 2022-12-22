FROM mcr.microsoft.com/dotnet/runtime:6.0-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build

WORKDIR /src/Sample.Platform.Contracts/
COPY Sample.Platform.Contracts/* /src/Sample.Platform.Contracts/

WORKDIR /src/Sample.Platform.Producer/
COPY Sample.Platform.Producer/*.csproj .
RUN dotnet restore /p:IsDockerBuild=true

COPY Sample.Platform.Producer .
RUN dotnet build -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "sample-producer.dll"]