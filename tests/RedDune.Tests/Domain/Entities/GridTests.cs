using RedDune.Core.Domain.Entities;
using RedDune.Core.Domain.ValueObjects;
using Xunit;

namespace RedDune.Tests.Domain.Entities;

public class GridTests
{
    [Fact]
    public void Create_ValidDimensions_ReturnsGrid()
    {
        var grid = new Grid(5, 3);

        Assert.Equal(5, grid.Width);
        Assert.Equal(3, grid.Height);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(-1, -1)]
    public void Create_InvalidDimensions_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Grid(width, height));
    }

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(5, 3, true)]
    [InlineData(-1, 0, false)]
    [InlineData(0, -1, false)]
    public void IsWithinBounds_ReturnsCorrectResult(int x, int y, bool expected)
    {
        var grid = new Grid(5, 3);
        var coordinates = Coordinates.Createunchecked(x, y);

        Assert.Equal(expected, grid.IsWithinBounds(coordinates));
    }

    [Fact]
    public void IsOnEdge_CornerPosition_ReturnsTrue()
    {
        var grid = new Grid(5, 3);
        var corner = Coordinates.Createunchecked(0, 0);

        Assert.True(grid.IsOnEdge(corner));
    }

    [Fact]
    public void IsOnEdge_CenterPosition_ReturnsFalse()
    {
        var grid = new Grid(5, 3);
        var center = Coordinates.Createunchecked(2, 1);

        Assert.False(grid.IsOnEdge(center));
    }

    [Fact]
    public void WouldFallOff_FacingNorthAtTop_ReturnsTrue()
    {
        var grid = new Grid(5, 3);
        var topPosition = Coordinates.Createunchecked(2, 3);

        Assert.True(grid.WouldFallOff(topPosition, Orientation.North));
    }

    [Fact]
    public void WouldFallOff_FacingNorthNotAtTop_ReturnsFalse()
    {
        var grid = new Grid(5, 3);
        var position = Coordinates.Createunchecked(2, 2);

        Assert.False(grid.WouldFallOff(position, Orientation.North));
    }

    [Fact]
    public void AddScent_AddsScentToGrid()
    {
        var grid = new Grid(5, 3);
        var position = Coordinates.Createunchecked(0, 2);
        grid.AddScent(position, Orientation.North);

        Assert.Single(grid.Scents);
    }

    [Fact]
    public void HasRelevantScent_WithMatchingScent_ReturnsTrue()
    {
        var grid = new Grid(5, 3);
        var position = Coordinates.Createunchecked(0, 2);
        grid.AddScent(position, Orientation.North);

        Assert.True(grid.HasRelevantScent(position, Orientation.North));
    }

    [Fact]
    public void HasRelevantScent_WithDifferentScent_ReturnsFalse()
    {
        var grid = new Grid(5, 3);
        var position = Coordinates.Createunchecked(0, 2);
        grid.AddScent(position, Orientation.North);

        Assert.False(grid.HasRelevantScent(position, Orientation.East));
    }

    [Fact]
    public void CanMoveSafely_NoFallOff_ReturnsTrue()
    {
        var grid = new Grid(5, 3);
        var position = Coordinates.Createunchecked(2, 2);

        Assert.True(grid.CanMoveSafely(position, Orientation.East));
    }

    [Fact]
    public void CanMoveSafely_WouldFallOffWithNoScent_ReturnsTrue()
    {
        var grid = new Grid(5, 3);
        var position = Coordinates.Createunchecked(5, 3);

        Assert.True(grid.CanMoveSafely(position, Orientation.North));
    }

    [Fact]
    public void CanMoveSafely_WouldFallOffWithRelevantScent_ReturnsFalse()
    {
        var grid = new Grid(5, 3);
        var position = Coordinates.Createunchecked(5, 3);
        grid.AddScent(position, Orientation.North);

        Assert.False(grid.CanMoveSafely(position, Orientation.North));
    }
}