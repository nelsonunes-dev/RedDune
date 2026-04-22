using RedDune.Core.Application.DTOs;
using RedDune.Core.Domain.Entities;
using RedDune.Core.Domain.ValueObjects;

namespace RedDune.Core.Application;

public sealed class SimulationEngine
{
    private readonly int _gridWidth;
    private readonly int _gridHeight;

    public SimulationEngine(int gridWidth, int gridHeight)
    {
        _gridWidth = gridWidth;
        _gridHeight = gridHeight;
    }

    public SimulationOutput Process(SimulationInput input)
    {
        var grid = new Grid(_gridWidth, _gridHeight);

        var outputs = new List<RobotOutput>();

        foreach (var robotInput in input.Robots)
        {
            var output = ProcessRobot(robotInput, grid);
            outputs.Add(output);
        }

        return new SimulationOutput(outputs);
    }

    private RobotOutput ProcessRobot(RobotInput robotInput, Grid grid)
    {
        var position = Coordinates.Create(robotInput.X, robotInput.Y);
        var orientation = OrientationExtensions.FromChar(robotInput.Orientation);
        var robot = Robot.Create(position, orientation);

        var commands = ParseCommands(robotInput.Commands);

        foreach (var command in commands)
        {
            if (command == Command.Forward)
            {
                if (grid.WouldFallOff(robot.Position, robot.Orientation))
                {
                    if (grid.CanMoveSafely(robot.Position, robot.Orientation))
                    {
                        grid.AddScent(robot.Position, robot.Orientation);
                        robot.MarkAsLost(robot.Position, robot.Orientation);
                        break;
                    }
                }
                else
                {
                    robot.ExecuteForward();
                }
            }
            else if (command == Command.Left)
            {
                robot.TurnLeft();
            }
            else if (command == Command.Right)
            {
                robot.TurnRight();
            }
        }

        return new RobotOutput(
            robot.Position.X,
            robot.Position.Y,
            robot.Orientation.ToChar(),
            robot.IsLost
        );
    }

    private static IReadOnlyList<Command> ParseCommands(string commandString)
    {
        var commands = new List<Command>();
        foreach (var c in commandString)
        {
            try
            {
                commands.Add(CommandExtensions.FromChar(c));
            }
            catch (ArgumentException)
            {
            }
        }
        return commands;
    }
}