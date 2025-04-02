namespace Sharpfish.Tests;

public class ResponseParserTests
{
    [Test]
    public void ParseReadyOK_WithValidResponse_ReturnsTrue()
    {
        // Act && Assert
        Assert.That(ResponseParser.ParseReadyOK("readyok"));
    }

    [Test]
    public void ParseReadyOK_WithInvalidResponse_ReturnsFalse()
    {
        // Act && Assert
        Assert.That(ResponseParser.ParseReadyOK(""), Is.False);
    }

    [Test]
    public void ParseBestMove_WithValidResponse_ReturnsBestMove()
    {
        // Arrange
        string response = "bestmove e2e4 ponder e7e5"; // Response from ucinewgame -> go depth 20

        // Act
        string result = ResponseParser.ParseBestMove(response);

        // Assert
        Assert.That(result, Is.EqualTo("e2e4"));
    }

    [Test]
    public void ParseBestMove_WithInvalidResponse_ThrowsException()
    {
        // Arrange
        string response = "info depth 20 score cp 32"; // 1 line too early

        // Act && Assert
        Assert.Throws<InvalidDataException>(() => ResponseParser.ParseBestMove(response));
    }

    [Test]
    public void ParseBestMove_WithNoneMove_ThrowsException()
    {
        // Arrange
        string response = "bestmove (none)";

        // Act && Assert
        Assert.Throws<Exception>(() => ResponseParser.ParseBestMove(response));
    }

    [Test]
    public void ParseBestMove_WithNullResponse_ThrowsArgumentNullException()
    {
        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => ResponseParser.ParseBestMove(null));
    }

    [Test]
    public void ParseEvaluation_WithValidResponse_ReturnsEvaluation()
    {
        // Arrange
        string response = "Final evaluation       +0.09 (white side) [with scaled NNUE, ...]";

        // Act && Assert
        Assert.That(ResponseParser.ParseEvaluation(response), Is.EqualTo("+0.09")); // 0.09 from eval after ucinewgame
    }

    [Test]
    public void ParseEvaluation_WithInvalidFormatReponse_ThrowsInvalidDataException()
    {
        // Arrange
        string response = "Final evaluation";

        // Act && Assert
        Assert.Throws<InvalidDataException>(() => ResponseParser.ParseEvaluation(response));
    }

    [Test]
    public void ParseEvaluation_WithNegativeScore_ReturnsNegativeValue()
    {
        // Arrange
        string response = "Final evaluation       -1.23 (black side) [with scaled NNUE, ...]";

        // Act
        string result = ResponseParser.ParseEvaluation(response);

        // Assert
        Assert.That(result, Is.EqualTo("-1.23"));
    }

    [Test]
    public void ParsePV_WithValidResponse_ReturnsPV()
    {
        // Arrange
        string response = "info depth 20 seldepth 31 multipv 1 score cp 24 nodes 631058 nps 653269 hashfull 239 tbhits 0 time 966 pv e2e4 e7e5 g1f3 g8f6 d2d4 f6e4 f3e5 d7d5 f1d3 b8d7 e5d7 d8d7 e1g1 f8d6";

        // Act
        string[] result = ResponseParser.ParsePV(response);

        // Assert
        Assert.That(result, Has.Length.EqualTo(14)); // e2e4 e7e5 g1f3 g8f6 d2d4 f6e4 f3e5 d7d5 f1d3 b8d7 e5d7 d8d7 e1g1 f8d6 = 14
    }

    [Test]
    public void ParsePV_WithNullResponse_ThrowsArgumentNullException()
    {
        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => ResponseParser.ParsePV(null));
    }
}
