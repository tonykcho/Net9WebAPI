Music Drive Database Migrations

## Get migrations list
    dotnet ef migrations list -p Net9WebAPI.DataAccess -s Net9WebAPI.API

## Add new migrations
    dotnet ef migrations add <name> -p Net9WebAPI.DataAccess -s Net9WebAPI.API -o ./Migrations -c Net9WebAPIDbContext

## Remove new migrations
    dotnet ef migrations remove -p Net9WebAPI.DataAccess -s Net9WebAPI.API -o ./Migrations -c Net9WebAPIDbContext

## Reset Database
    dotnet ef migrations update 0 -p Net9WebAPI.DataAccess -s Net9WebAPI.API -o ./Migrations -c Net9WebAPIDbContext