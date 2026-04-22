using RedDune.Core.Domain.ValueObjects;

namespace RedDune.Core.Domain.Entities;

public sealed class Robot
{
    private readonly List<Command> _commands = new();

    public Guid Id { get; }
    public Coordinates Position { get; private set; }
    public Orientation Orientation { get; private set; }
    public bool IsLost { get; private set; }

    public IReadOnlyCollection<Command> Commands => _commands.AsReadOnly();

    private Robot(Guid id, Coordinates position, Orientation orientation)
    {
        Id = id;
        Position = position;
        Orientation = orientation;
        IsLost = false;
    }

    public static Robot Create(Coordinates position, Orientation orientation)
    {
        return new Robot(Guid.NewGuid(), position, orientation);
    }

    public static Robot Create(int x, int y, char orientationChar)
    {
        var position = Coordinates.Create(x, y);
        var orientation = OrientationExtensions.FromChar(orientationChar);
        return new Robot(Guid.NewGuid(), position, orientation);
    }

    public void AddCommands(IEnumerable<Command> commands)
    {
        _commands.Clear();
        _commands.AddRange(commands);
    }

    public void ExecuteForward()
    {
        Position = Position.MoveForward(Orientation);
    }

    public void TurnLeft()
    {
        Orientation = Orientation.TurnLeft();
    }

    public void TurnRight()
    {
        Orientation = Orientation.TurnRight();
    }

    public void MarkAsLost(Coordinates lastPosition, Orientation lastOrientation)
    {
        IsLost = true;
        Position = lastPosition;
        Orientation = lastOrientation;
    }

    public override string ToString() => IsLost
        ? $"{Position} {Orientation.ToChar()} LOST"
        : $"{Position} {Orientation.ToChar()}";
}