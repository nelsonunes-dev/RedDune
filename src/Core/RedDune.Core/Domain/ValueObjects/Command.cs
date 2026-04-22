namespace RedDune.Core.Domain.ValueObjects;

public enum Command
{
    Left,
    Right,
    Forward
}

public static class CommandExtensions
{
    public static char ToChar(this Command command)
    {
        return command switch
        {
            Command.Left => 'L',
            Command.Right => 'R',
            Command.Forward => 'F',
            _ => throw new ArgumentOutOfRangeException(nameof(command))
        };
    }

    public static Command FromChar(char c)
    {
        return char.ToUpperInvariant(c) switch
        {
            'L' => Command.Left,
            'R' => Command.Right,
            'F' => Command.Forward,
            _ => throw new ArgumentException($"Invalid command character: {c}", nameof(c))
        };
    }
}