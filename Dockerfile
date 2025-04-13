FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY Net9WebAPI.sln .
COPY ["src/Net9WebAPI.WebAPI", "Net9WebAPI.WebAPI/"]
COPY ["src/Net9WebAPI.Application", "Net9WebAPI.Application/"]
COPY ["src/Net9WebAPI.DataAccess", "Net9WebAPI.DataAccess/"]
COPY ["src/Net9WebAPI.Domain", "Net9WebAPI.Domain/"]

RUN dotnet restore "Net9WebAPI.WebAPI/Net9WebAPI.WebAPI.csproj"
WORKDIR "/src/Net9WebAPI.WebAPI/"
RUN dotnet build "Net9WebAPI.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Net9WebAPI.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Net9WebAPI.WebAPI.dll"]




