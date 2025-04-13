var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgres");
var password = builder.AddParameter("password", "postgres");
var postgres = builder.AddPostgres("postgres", username, password, 5432)
    .WithLifetime(ContainerLifetime.Persistent);

var postgresDatabase = postgres.AddDatabase("Net9WebAPI");

builder.AddProject<Projects.Net9WebAPI_WebAPI>("api")
    .WithExternalHttpEndpoints()
    .WithHttpsHealthCheck("/health")
    .WithReference(postgresDatabase)
    .WaitFor(postgresDatabase);

builder.Build().Run();
