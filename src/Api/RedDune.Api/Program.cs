using RedDune.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapSimulationEndpoints();

app.Run();