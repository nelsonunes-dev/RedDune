namespace RedDune.Core.Domain.ValueObjects;

public enum Orientation
{
    North,
    East,
    South,
    West
}

public static class OrientationExtensions
{
    public static Orientation TurnLeft(this Orientation orientation)
    {
        return orientation switch
        {
            Orientation.North => Orientation.West,
            Orientation.West => Orientation.South,
            Orientation.South => Orientation.East,
            Orientation.East => Orientation.North,
            _ => throw new ArgumentOutOfRangeException(nameof(orientation))
        };
    }

    public static Orientation TurnRight(this Orientation orientation)
    {
        return orientation switch
        {
            Orientation.North => Orientation.East,
            Orientation.East => Orientation.South,
            Orientation.South => Orientation.West,
            Orientation.West => Orientation.North,
            _ => throw new ArgumentOutOfRangeException(nameof(orientation))
        };
    }

    public static char ToChar(this Orientation orientation)
    {
        return orientation switch
        {
            Orientation.North => 'N',
            Orientation.East => 'E',
            Orientation.South => 'S',
            Orientation.West => 'W',
            _ => throw new ArgumentOutOfRangeException(nameof(orientation))
        };
    }

    public static Orientation FromChar(char c)
    {
        return char.ToUpperInvariant(c) switch
        {
            'N' => Orientation.North,
            'E' => Orientation.East,
            'S' => Orientation.South,
            'W' => Orientation.West,
            _ => throw new ArgumentException($"Invalid orientation character: {c}", nameof(c))
        };
    }
}