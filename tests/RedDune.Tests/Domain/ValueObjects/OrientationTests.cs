using RedDune.Core.Domain.ValueObjects;
using Xunit;

namespace RedDune.Tests.Domain.ValueObjects;

public class OrientationTests
{
    [Theory]
    [InlineData(Orientation.North, Orientation.West)]
    [InlineData(Orientation.West, Orientation.South)]
    [InlineData(Orientation.South, Orientation.East)]
    [InlineData(Orientation.East, Orientation.North)]
    public void TurnLeft_ReturnsCorrectOrientation(Orientation current, Orientation expected)
    {
        Assert.Equal(expected, current.TurnLeft());
    }

    [Theory]
    [InlineData(Orientation.North, Orientation.East)]
    [InlineData(Orientation.East, Orientation.South)]
    [InlineData(Orientation.South, Orientation.West)]
    [InlineData(Orientation.West, Orientation.North)]
    public void TurnRight_ReturnsCorrectOrientation(Orientation current, Orientation expected)
    {
        Assert.Equal(expected, current.TurnRight());
    }

    [Theory]
    [InlineData(Orientation.North, 'N')]
    [InlineData(Orientation.East, 'E')]
    [InlineData(Orientation.South, 'S')]
    [InlineData(Orientation.West, 'W')]
    public void ToChar_ReturnsCorrectCharacter(Orientation orientation, char expected)
    {
        Assert.Equal(expected, orientation.ToChar());
    }

    [Theory]
    [InlineData('N', Orientation.North)]
    [InlineData('E', Orientation.East)]
    [InlineData('S', Orientation.South)]
    [InlineData('W', Orientation.West)]
    [InlineData('n', Orientation.North)]
    [InlineData('e', Orientation.East)]
    public void FromChar_ValidCharacter_ReturnsOrientation(char input, Orientation expected)
    {
        Assert.Equal(expected, OrientationExtensions.FromChar(input));
    }

    [Theory]
    [InlineData('X')]
    [InlineData('1')]
    public void FromChar_InvalidCharacter_ThrowsArgumentException(char input)
    {
        Assert.Throws<ArgumentException>(() => OrientationExtensions.FromChar(input));
    }
}