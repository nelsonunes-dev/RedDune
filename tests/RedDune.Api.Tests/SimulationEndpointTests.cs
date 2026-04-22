using Microsoft.AspNetCore.Mvc.Testing;
using RedDune.Api.Requests;
using RedDune.Api.Responses;
using Xunit;

namespace RedDune.Api.Tests;

public class SimulationEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SimulationEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Post_SingleRobot_ReturnsCorrectOutput()
    {
        var client = _factory.CreateClient();
        var request = new SimulationRequest(5, 3, new[]
        {
            new RobotRequest(1, 1, 'N', "F")
        });

        var response = await client.PostAsJsonAsync("/api/simulation", request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SimulationResponse>();

        Assert.NotNull(result);
        Assert.Single(result.Robots);
        Assert.Equal(1, result.Robots[0].X);
        Assert.Equal(2, result.Robots[0].Y);
        Assert.Equal('N', result.Robots[0].Orientation);
        Assert.False(result.Robots[0].IsLost);
    }

    [Fact]
    public async Task Post_RobotFallsOff_ReturnsLost()
    {
        var client = _factory.CreateClient();
        var request = new SimulationRequest(5, 3, new[]
        {
            new RobotRequest(3, 3, 'N', "F")
        });

        var response = await client.PostAsJsonAsync("/api/simulation", request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SimulationResponse>();

        Assert.NotNull(result);
        Assert.True(result.Robots[0].IsLost);
    }

    [Fact]
    public async Task Post_SecondRobotBlockedByScent_ReturnsNotLost()
    {
        var client = _factory.CreateClient();
        var request = new SimulationRequest(5, 3, new[]
        {
            new RobotRequest(3, 3, 'N', "F"),
            new RobotRequest(3, 3, 'N', "F")
        });

        var response = await client.PostAsJsonAsync("/api/simulation", request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SimulationResponse>();

        Assert.NotNull(result);
        Assert.Equal(2, result.Robots.Count);
        Assert.True(result.Robots[0].IsLost);
        Assert.False(result.Robots[1].IsLost);
    }

    [Fact]
    public async Task Post_EmptyRobots_ReturnsEmptyArray()
    {
        var client = _factory.CreateClient();
        var request = new SimulationRequest(5, 3, Array.Empty<RobotRequest>());

        var response = await client.PostAsJsonAsync("/api/simulation", request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SimulationResponse>();

        Assert.NotNull(result);
        Assert.Empty(result.Robots);
    }

    [Fact]
    public async Task Post_InvalidGridDimensions_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var request = new SimulationRequest(-1, 3, Array.Empty<RobotRequest>());

        var response = await client.PostAsJsonAsync("/api/simulation", request);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_MultipleRobots_ReturnsSequentialResults()
    {
        var client = _factory.CreateClient();
        var request = new SimulationRequest(5, 3, new[]
        {
            new RobotRequest(1, 1, 'E', "RFRF"),
            new RobotRequest(2, 2, 'N', "FF")
        });

        var response = await client.PostAsJsonAsync("/api/simulation", request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SimulationResponse>();

        Assert.NotNull(result);
        Assert.Equal(2, result.Robots.Count);
    }
}