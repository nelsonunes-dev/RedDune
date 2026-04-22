namespace RedDune.Core.Domain.ValueObjects;

public sealed class Coordinates : IEquatable<Coordinates>
{
    public int X { get; }
    public int Y { get; }

    private Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Coordinates Create(int x, int y)
    {
        if (x < 0)
            throw new ArgumentOutOfRangeException(nameof(x), "X coordinate must be non-negative");
        if (y < 0)
            throw new ArgumentOutOfRangeException(nameof(y), "Y coordinate must be non-negative");

        return new Coordinates(x, y);
    }

    public static Coordinates Createunchecked(int x, int y) => new(x, y);

    public Coordinates MoveForward(Orientation orientation)
    {
        return orientation switch
        {
            Orientation.North => new Coordinates(X, Y + 1),
            Orientation.East => new Coordinates(X + 1, Y),
            Orientation.South => new Coordinates(X, Y - 1),
            Orientation.West => new Coordinates(X - 1, Y),
            _ => throw new ArgumentOutOfRangeException(nameof(orientation))
        };
    }

    public bool Equals(Coordinates? other) => other is not null && X == other.X && Y == other.Y;

    public override bool Equals(object? obj) => Equals(obj as Coordinates);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"({X}, {Y})";

    public static bool operator ==(Coordinates? left, Coordinates? right) => Equals(left, right);
    public static bool operator !=(Coordinates? left, Coordinates? right) => !Equals(left, right);
}