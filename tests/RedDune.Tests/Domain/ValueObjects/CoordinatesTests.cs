using RedDune.Core.Domain.ValueObjects;
using Xunit;

namespace RedDune.Tests.Domain.ValueObjects;

public class CoordinatesTests
{
    [Fact]
    public void Create_ValidCoordinates_ReturnsCoordinates()
    {
        var coordinates = Coordinates.Create(2, 3);

        Assert.Equal(2, coordinates.X);
        Assert.Equal(3, coordinates.Y);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(-1, -1)]
    public void Create_InvalidCoordinates_ThrowsArgumentOutOfRangeException(int x, int y)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Coordinates.Create(x, y));
    }

    [Theory]
    [InlineData(0, 0, Orientation.North, 0, 1)]
    [InlineData(0, 0, Orientation.East, 1, 0)]
    [InlineData(0, 0, Orientation.South, 0, -1)]
    [InlineData(0, 0, Orientation.West, -1, 0)]
    public void MoveForward_ReturnsCorrectNewPosition(int startX, int startY, Orientation orientation, int expectedX, int expectedY)
    {
        var coordinates = Coordinates.Create(startX, startY);
        var newPosition = coordinates.MoveForward(orientation);

        Assert.Equal(expectedX, newPosition.X);
        Assert.Equal(expectedY, newPosition.Y);
    }

    [Fact]
    public void Equals_SameCoordinates_ReturnsTrue()
    {
        var coord1 = Coordinates.Create(2, 3);
        var coord2 = Coordinates.Create(2, 3);

        Assert.True(coord1.Equals(coord2));
        Assert.True(coord1 == coord2);
        Assert.False(coord1 != coord2);
    }

    [Fact]
    public void Equals_DifferentCoordinates_ReturnsFalse()
    {
        var coord1 = Coordinates.Create(2, 3);
        var coord2 = Coordinates.Create(3, 2);

        Assert.False(coord1.Equals(coord2));
        Assert.False(coord1 == coord2);
        Assert.True(coord1 != coord2);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var coordinates = Coordinates.Create(2, 3);
        Assert.Equal("(2, 3)", coordinates.ToString());
    }
}