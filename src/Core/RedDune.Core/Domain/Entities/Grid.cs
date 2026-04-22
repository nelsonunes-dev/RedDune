using RedDune.Core.Domain.ValueObjects;

namespace RedDune.Core.Domain.Entities;

public sealed class Grid
{
    private readonly HashSet<Scent> _scents = new();

    public int Width { get; }
    public int Height { get; }

    public IReadOnlyCollection<Scent> Scents => _scents.ToList().AsReadOnly();

    public Grid(int width, int height)
    {
        if (width < 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be non-negative");
        if (height < 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be non-negative");

        Width = width;
        Height = height;
    }

    public bool IsWithinBounds(Coordinates position)
    {
        return position.X >= 0 && position.X <= Width && position.Y >= 0 && position.Y <= Height;
    }

    public bool IsOnEdge(Coordinates position)
    {
        return position.X == 0 || position.X == Width || position.Y == 0 || position.Y == Height;
    }

    public bool WouldFallOff(Coordinates currentPosition, Orientation orientation)
    {
        var nextPosition = currentPosition.MoveForward(orientation);
        return !IsWithinBounds(nextPosition);
    }

    public void AddScent(Coordinates position, Orientation orientation)
    {
        _scents.Add(new Scent(position, orientation));
    }

    public bool HasRelevantScent(Coordinates position, Orientation orientation)
    {
        return _scents.Any(s => s.IsRelevantFor(position, orientation));
    }

    public bool CanMoveSafely(Coordinates position, Orientation orientation)
    {
        if (!WouldFallOff(position, orientation))
            return true;

        return !HasRelevantScent(position, orientation);
    }
}