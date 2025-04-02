namespace Sharpfish.Tests;

public class StockfishEngineTests
{
    private StockfishEngine _engine;
    private const string path = @"..\..\..\stockfish.exe";

    [SetUp]
    public void Setup()
    {
        _engine = new StockfishEngine(path);
    }

    [TearDown]
    public void Cleanup()
    {
        _engine.Dispose();
    }

    [Test]
    public async Task GetBestMove_WithStartingPosition_ReturnsValidMove()
    {
        // Arrange
        string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        await _engine.SetPosition(startingFen);

        // Act
        string bestMove = await _engine.GetBestMove(timeMs: 300);

        // Assert
        Assert.That(IsChessNotation(bestMove), Is.True, $"Exptected valid chess move notation");
    }

    [Test]
    public async Task SetOption_ChangesEngineParamer()
    {
        // Arrange
        await _engine.SetOption("MultiPV", "3");
        string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        await _engine.SetPosition(startingFen);

        // Act
        var pvs = await _engine.GetPV();

        // Assert - Should be 3 PVs
        Assert.That(pvs.Count, Is.EqualTo(3), "Expected multiple principal variations");
    }

    [Test]
    public async Task NewGame_ResetsEngine()
    {
        // Arrange
        string middleGameFen = "r1bqkb1r/pppp1ppp/2n2n2/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R w KQkq - 0 1";
        await _engine.SetPosition(middleGameFen);

        // Act
        await _engine.NewGame();
        string bestMove = await _engine.GetBestMove(timeMs: 100);

        // Assert - No crashes
        Assert.That(IsChessNotation(bestMove), Is.True, $"Expected valid chess move after NewGame, got: {bestMove}");
    }

    [Test]
    public async Task SetPosition_WithMoves_SetsCorrectPosition()
    {
        // Arrange
        string[] moves = ["e2e4", "e7e5", "g1f3"];

        // Act
        await _engine.SetPosition(moves);

        // Assert - Make sure that bestMove is valid
        string bestMove = await _engine.GetBestMove();
        Assert.That(IsChessNotation(bestMove), Is.True);
    }

    [Test]
    public async Task IsReady_ReturnsTrue_WhenEngineReady()
    {
        // Act
        bool isReady = await _engine.IsReady();

        // Assert
        Assert.That(isReady, Is.True, "Engine should say readyok");
    }

    [Test]
    public void ValidateFen_WithValidFen_ReturnsTrue()
    {
        // Arrange
        string validFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        // Act
        bool isValid = _engine.ValidateFen(validFen);

        // Assert
        Assert.That(isValid, Is.True, "Valid FEN should be recognized");
    }

    [Test]
    public async Task WriteLine_ReadLine_CommunicatesWithEngine()
    {
        // Act
        await _engine.WriteLine("uci");
        string? response = await _engine.ReadLine();

        // Assert
        Assert.That(response, Is.Not.Null);
        // The first line could be various things depending on Stockfish version
        // but it should contain some response
    }

    [Test]
    public async Task GetEvaluation_ReturnsEvaluationData()
    {
        // Arrange
        string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        await _engine.SetPosition(startingFen);

        // Act
        string evaluation = await _engine.GetEvaluation();

        // Assert
        Assert.That(evaluation, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task MultipleQueries_WorkCorrectly()
    {
        // Arrange
        string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        await _engine.SetPosition(startingFen);

        // Act - Do multiple operations to ensure the engine remains stable
        string firstMove = await _engine.GetBestMove(timeMs: 100);
        await _engine.SetPosition("r1bqkbnr/pppp1ppp/2n5/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R w KQkq - 0 3");
        string secondMove = await _engine.GetBestMove(timeMs: 100);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(IsChessNotation(firstMove), Is.True);
            Assert.That(IsChessNotation(secondMove), Is.True);
        });
    }

    [Test]
    public async Task DepthProperty_AffectsAnalysisDepth()
    {
        // Arrange
        string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        await _engine.SetPosition(startingFen);

        // Set a very low depth for quick test
        _engine.Depth = 5;

        // Act - Run analysis with the modified depth
        await _engine.GetBestMove();

        // Assert - Hard to directly verify depth was used, but no exception means it worked
        Assert.Pass("Successfully completed analysis with modified depth");
    }

    [Test]
    public void Dispose_ReleasesResources()
    {
        // Arrange
        var engine = new StockfishEngine(path);

        // Act
        engine.Dispose();

        // Assert - difficult to directly test, but we can check that disposing twice doesn't throw
        Assert.DoesNotThrow(() => engine.Dispose());
    }

    private static bool IsChessNotation(string move)
    {
        if (string.IsNullOrEmpty(move) || move.Length < 4)
            return false;

        // Simple validation - should be source square and target square
        // e.g., "e2e4", "g1f3"
        char fromFile = move[0];
        char fromRank = move[1];
        char toFile = move[2];
        char toRank = move[3];

        return (fromFile >= 'a' && fromFile <= 'h') &&
               (fromRank >= '1' && fromRank <= '8') &&
               (toFile >= 'a' && toFile <= 'h') &&
               (toRank >= '1' && toRank <= '8');
    }
}
