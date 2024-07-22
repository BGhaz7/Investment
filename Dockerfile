FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /investment
EXPOSE 5013

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Investment.API/Investment.API.csproj", "Investment.API/"]
COPY ["Investment.Models/Investment.Models.csproj", "Investment.Models/"]
COPY ["Investment.Repository/Investment.Repository.csproj", "Investment.Repository/"]
COPY ["Investment.Service/Investment.Service.csproj", "Investment.Service/"]
RUN dotnet restore "Investment.API/Investment.API.csproj"
COPY . .
WORKDIR "/src/Investment.API"
RUN dotnet build "Investment.API.csproj" -c $BUILD_CONFIGURATION -o /investment/build

RUN dotnet tool install --global dotnet-ef

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Investment.API.csproj" -c $BUILD_CONFIGURATION -o /investment/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /investment
COPY --from=publish /investment/publish .
ENTRYPOINT ["dotnet", "Investment.API.dll"]