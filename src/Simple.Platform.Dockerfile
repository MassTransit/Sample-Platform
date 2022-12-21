FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src/Sample.Platform.Contracts/
COPY Sample.Platform.Contracts/* /src/Sample.Platform.Contracts/

WORKDIR /src/Sample.Platform/
COPY Sample.Platform/*.csproj .
RUN dotnet restore -r linux-musl-x64

COPY Sample.Platform .
RUN dotnet publish -c Release -o /app -r linux-musl-x64 --no-restore

FROM masstransit/platform:latest
WORKDIR /app
ARG MT_APP=/app
ENV MT_APP="${MT_APP}"
COPY --from=build /app ./


