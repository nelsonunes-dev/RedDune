using RedDune.Api.Requests;
using RedDune.Api.Responses;
using RedDune.Core.Application;
using RedDune.Core.Application.DTOs;

namespace RedDune.Api.Endpoints;

public static class SimulationEndpoints
{
    public static void MapSimulationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/simulation", Simulate);
    }

    private static IResult Simulate(SimulationRequest request)
    {
        if (request.GridWidth < 0 || request.GridHeight < 0)
        {
            return Results.BadRequest("Grid dimensions must be non-negative");
        }

        if (request.Robots == null || request.Robots.Count == 0)
        {
            return Results.Ok(new SimulationResponse(Array.Empty<RobotResponse>()));
        }

        var engine = new SimulationEngine(request.GridWidth, request.GridHeight);

        var input = new SimulationInput(
            request.GridWidth,
            request.GridHeight,
            request.Robots.Select(r => new RobotInput(r.X, r.Y, r.Orientation, r.Commands)).ToList()
        );

        var output = engine.Process(input);

        var response = new SimulationResponse(
            output.Robots.Select(r => new RobotResponse(r.X, r.Y, r.Orientation, r.IsLost)).ToList()
        );

        return Results.Ok(response);
    }
}