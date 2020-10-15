#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY PocStubAppo1.csproj ./
RUN dotnet restore "./PocStubAppo1.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PocStubAppo1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PocStubAppo1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PocStubAppo1.dll"]
