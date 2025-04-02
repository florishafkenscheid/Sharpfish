using System.Runtime.InteropServices;

namespace Sharpfish.Tests;

public class CommandBuilderTests
{
    [Test]
    public void Position_ReturnsCorrectFormatWithFen()
    {
        // Arrange
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        // Act
        string result = CommandBuilder.Position(fen);

        // Assert
        Assert.That(result, Is.EqualTo("position fen " + fen));
    }

    [Test]
    public void SetOption_ReturnsCorrectFormat()
    {
        // Act
        string result = CommandBuilder.SetOption("MultiPV", "3");

        // Assert
        Assert.That(result, Is.EqualTo("setoption name MultiPV value 3"));
    }

    [Test]
    public void Go_WithDepth_ReturnsCorrectFormat()
    {
        // Act
        string result = CommandBuilder.Go(Depth: 15);

        // Assert
        Assert.That(result, Is.EqualTo("go depth 15"));
    }

    [Test]
    public void Go_WithTime_ReturnsCorrectFormat()
    {
        // Act
        string result = CommandBuilder.Go(timeMs: 1000);

        // Assert
        Assert.That(result, Is.EqualTo("go movetime 1000"));
    }
}
