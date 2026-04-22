using RedDune.Core.Domain.Entities;
using RedDune.Core.Domain.ValueObjects;
using Xunit;

namespace RedDune.Tests.Domain.Entities;

public class RobotTests
{
    [Fact]
    public void Create_ValidInputs_ReturnsRobot()
    {
        var position = Coordinates.Create(1, 2);
        var robot = Robot.Create(position, Orientation.East);

        Assert.Equal(1, robot.Position.X);
        Assert.Equal(2, robot.Position.Y);
        Assert.Equal(Orientation.East, robot.Orientation);
        Assert.False(robot.IsLost);
    }

    [Fact]
    public void Create_FromIntAndChar_ReturnsRobot()
    {
        var robot = Robot.Create(1, 2, 'N');

        Assert.Equal(1, robot.Position.X);
        Assert.Equal(2, robot.Position.Y);
        Assert.Equal(Orientation.North, robot.Orientation);
        Assert.False(robot.IsLost);
    }

    [Fact]
    public void ExecuteForward_MovesNorth_IncreasesY()
    {
        var robot = Robot.Create(1, 1, 'N');
        robot.ExecuteForward();

        Assert.Equal(1, robot.Position.X);
        Assert.Equal(2, robot.Position.Y);
        Assert.Equal(Orientation.North, robot.Orientation);
    }

    [Fact]
    public void ExecuteForward_MovesEast_IncreasesX()
    {
        var robot = Robot.Create(1, 1, 'E');
        robot.ExecuteForward();

        Assert.Equal(2, robot.Position.X);
        Assert.Equal(1, robot.Position.Y);
        Assert.Equal(Orientation.East, robot.Orientation);
    }

    [Fact]
    public void TurnLeft_ChangesOrientationCorrectly()
    {
        var robot = Robot.Create(1, 1, 'N');
        robot.TurnLeft();

        Assert.Equal(Orientation.West, robot.Orientation);
    }

    [Fact]
    public void TurnRight_ChangesOrientationCorrectly()
    {
        var robot = Robot.Create(1, 1, 'N');
        robot.TurnRight();

        Assert.Equal(Orientation.East, robot.Orientation);
    }

    [Fact]
    public void MarkAsLost_SetsLostState()
    {
        var robot = Robot.Create(1, 1, 'N');
        var lastPosition = Coordinates.Create(1, 1);
        robot.MarkAsLost(lastPosition, Orientation.North);

        Assert.True(robot.IsLost);
        Assert.Equal(1, robot.Position.X);
        Assert.Equal(1, robot.Position.Y);
    }

    [Fact]
    public void ToString_NotLost_ReturnsPositionAndOrientation()
    {
        var robot = Robot.Create(1, 2, 'N');
        Assert.Equal("(1, 2) N", robot.ToString());
    }

    [Fact]
    public void ToString_IsLost_ReturnsPositionOrientationAndLost()
    {
        var robot = Robot.Create(1, 2, 'N');
        robot.MarkAsLost(Coordinates.Create(1, 2), Orientation.North);
        Assert.Equal("(1, 2) N LOST", robot.ToString());
    }

    [Fact]
    public void AddCommands_AddsCommandsToRobot()
    {
        var robot = Robot.Create(1, 1, 'N');
        robot.AddCommands(new[] { Command.Left, Command.Forward, Command.Right });

        Assert.Equal(3, robot.Commands.Count);
    }
}