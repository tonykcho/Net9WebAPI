FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8000
EXPOSE 8001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY Net9WebAPI.sln .
COPY ["src/Net9WebAPI.API", "Net9WebAPI.API/"]
COPY ["src/Net9WebAPI.Application", "Net9WebAPI.Application/"]
COPY ["src/Net9WebAPI.DataAccess", "Net9WebAPI.DataAccess/"]
COPY ["src/Net9WebAPI.Domain", "Net9WebAPI.Domain/"]

RUN dotnet restore "Net9WebAPI.API/Net9WebAPI.API.csproj"
WORKDIR "/src/Net9WebAPI.API/"
RUN dotnet build "Net9WebAPI.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Net9WebAPI.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Net9WebAPI.API.dll"]




