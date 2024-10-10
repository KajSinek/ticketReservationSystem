#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoreApi/TRS.Api/TRS.Api.csproj", "CoreApi/TRS.Api/"]
COPY ["CoreApi/TRS.CoreApi/TRS.CoreApi.csproj", "CoreApi/TRS.CoreApi/"]
COPY ["Helpers/Helpers.csproj", "Helpers/"]
COPY ["CoreApi/TRS.Tests/TRS.Tests.csproj", "CoreApi/TRS.Tests/"]
RUN dotnet restore "./CoreApi/TRS.Api/TRS.Api.csproj"
COPY . .
WORKDIR "/src/CoreApi/TRS.Api"
RUN dotnet build "./TRS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TRS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TRS.Api.dll"]