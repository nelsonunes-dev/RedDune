using RedDune.Core.Application;
using RedDune.Core.Application.DTOs;
using Xunit;

namespace RedDune.Tests.Application;

public class SimulationEngineTests
{
    [Fact]
    public void Process_EmptyRobotList_ReturnsEmptyOutput()
    {
        var engine = new SimulationEngine(5, 3);
        var input = new SimulationInput(5, 3, Array.Empty<RobotInput>());

        var output = engine.Process(input);

        Assert.Empty(output.Robots);
    }

    [Fact]
    public void Process_SingleRobotForward_UpdatesPosition()
    {
        var engine = new SimulationEngine(5, 3);
        var input = new SimulationInput(5, 3, new[]
        {
            new RobotInput(1, 1, 'N', "F")
        });

        var output = engine.Process(input);

        Assert.Single(output.Robots);
        Assert.Equal(1, output.Robots[0].X);
        Assert.Equal(2, output.Robots[0].Y);
        Assert.Equal('N', output.Robots[0].Orientation);
        Assert.False(output.Robots[0].IsLost);
    }

    [Fact]
    public void Process_SingleRobotTurns_UpdatesOrientation()
    {
        var engine = new SimulationEngine(5, 3);
        var input = new SimulationInput(5, 3, new[]
        {
            new RobotInput(1, 1, 'N', "L")
        });

        var output = engine.Process(input);

        Assert.Single(output.Robots);
        Assert.Equal('W', output.Robots[0].Orientation);
        Assert.False(output.Robots[0].IsLost);
    }

    [Fact]
    public void Process_RobotFallsOff_ReturnsLost()
    {
        var engine = new SimulationEngine(5, 3);
        var input = new SimulationInput(5, 3, new[]
        {
            new RobotInput(3, 3, 'N', "F")
        });

        var output = engine.Process(input);

        Assert.Single(output.Robots);
        Assert.True(output.Robots[0].IsLost);
        Assert.Equal(3, output.Robots[0].X);
        Assert.Equal(3, output.Robots[0].Y);
        Assert.Equal('N', output.Robots[0].Orientation);
    }

    [Fact]
    public void Process_RobotBlockedByScent_DoesNotFallOff()
    {
        var engine = new SimulationEngine(5, 3);
        var input = new SimulationInput(5, 3, new[]
        {
            new RobotInput(3, 3, 'N', "F"),
            new RobotInput(3, 3, 'N', "F")
        });

        var output = engine.Process(input);

        Assert.Equal(2, output.Robots.Count);
        Assert.True(output.Robots[0].IsLost);
        Assert.False(output.Robots[1].IsLost);
    }

    [Fact]
    public void Process_ClassicScenario_ProducesCorrectOutput()
    {
        var engine = new SimulationEngine(5, 3);
        var input = new SimulationInput(5, 3, new[]
        {
            new RobotInput(1, 1, 'E', "RFRFFFRF"),
            new RobotInput(3, 2, 'N', "FLLFLLF"),
            new RobotInput(0, 3, 'W', "LLFFFLFLF")
        });

        var output = engine.Process(input);

        Assert.Equal(3, output.Robots.Count);
        Assert.True(output.Robots[0].IsLost);
        Assert.False(output.Robots[1].IsLost);
        Assert.True(output.Robots[2].IsLost);
    }

    [Fact]
    public void Process_MultipleRobots_ProcessedSequentially()
    {
        var engine = new SimulationEngine(5, 3);
        var input = new SimulationInput(5, 3, new[]
        {
            new RobotInput(2, 2, 'N', "F"),
            new RobotInput(1, 1, 'E', "RFRF")
        });

        var output = engine.Process(input);

        Assert.Equal(2, output.Robots.Count);
        Assert.Equal(2, output.Robots[0].X);
        Assert.Equal(3, output.Robots[0].Y);
    }

    [Fact]
    public void Process_RobotStaysOnGrid_NotLost()
    {
        var engine = new SimulationEngine(5, 3);
        var input = new SimulationInput(5, 3, new[]
        {
            new RobotInput(0, 0, 'N', "FFF")
        });

        var output = engine.Process(input);

        Assert.Single(output.Robots);
        Assert.False(output.Robots[0].IsLost);
    }
}