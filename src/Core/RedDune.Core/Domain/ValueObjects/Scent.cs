namespace RedDune.Core.Domain.ValueObjects;

public sealed class Scent : IEquatable<Scent>
{
    public Coordinates Position { get; }
    public Orientation LostFacing { get; }

    public Scent(Coordinates position, Orientation lostFacing)
    {
        Position = position ?? throw new ArgumentNullException(nameof(position));
        LostFacing = lostFacing;
    }

    public bool IsRelevantFor(Coordinates position, Orientation orientation)
    {
        return Position.Equals(position) && LostFacing == orientation;
    }

    public bool Equals(Scent? other) => other is not null && Position.Equals(other.Position) && LostFacing == other.LostFacing;

    public override bool Equals(object? obj) => Equals(obj as Scent);

    public override int GetHashCode() => HashCode.Combine(Position, LostFacing);

    public override string ToString() => $"{Position} {LostFacing.ToChar()}";

    public static bool operator ==(Scent? left, Scent? right) => Equals(left, right);
    public static bool operator !=(Scent? left, Scent? right) => !Equals(left, right);
}