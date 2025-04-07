using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharpfish
{
    /// <summary>
    /// Defines the interface for interacting with the Stockfish chess engine.
    /// </summary>
    public interface IStockfishEngine : IDisposable
    {
        /// <summary>
        /// Gets or sets the default search depth (in plies) for move calculations.
        /// </summary>
        int Depth { get; set; }

        /// <summary>
        /// Gets or sets the number of principal variations (MultiPV) to output.
        /// </summary>
        int MultiPV { get; set; }

        /// <summary>
        /// Instructs the engine to start a new game.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task NewGame();

        /// <summary>
        /// Sets the current position using a FEN string.
        /// </summary>
        /// <param name="fen">The FEN string describing the chess position.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown if the FEN is invalid.</exception>
        Task SetPosition(string fen);

        /// <summary>
        /// Sets the current position from the starting position and applies a sequence of moves.
        /// </summary>
        /// <param name="moves">An array of moves in UCI format.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SetPosition(string[] moves);

        /// <summary>
        /// Retrieves the engine's evaluation of the current position.
        /// </summary>
        /// <returns>A task that resolves to the evaluation result as a string.</returns>
        Task<string> GetEvaluation();

        /// <summary>
        /// Retrieves the best move for the current position.
        /// </summary>
        /// <param name="timeMs">Optional time limit for the search in milliseconds. If null, uses the default depth.</param>
        /// <returns>A task that resolves to the best move in UCI format.</returns>
        Task<string> GetBestMove(double? timeMs = null);

        /// <summary>
        /// Sets a UCI option value.
        /// </summary>
        /// <param name="key">The name of the option.</param>
        /// <param name="value">The value to assign.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SetOption(string key, string value);

        /// <summary>
        /// Checks if the engine is ready to receive commands.
        /// </summary>
        /// <returns>A task that resolves to <c>true</c> if the engine is ready; otherwise, <c>false</c>.</returns>
        Task<bool> IsReady();

        /// <summary>
        /// Reads engine output until one of the expected strings is found.
        /// </summary>
        /// <param name="expected">Strings to look for in the engine's response.</param>
        /// <returns>A task that resolves to the line containing the expected string(s).</returns>
        /// <exception cref="TimeoutException">Thrown if the operation times out.</exception>
        Task<string> ReadUntil(params string[] expected);

        /// <summary>
        /// Reads a single line of output from the engine.
        /// </summary>
        /// <returns>A task that resolves to the read line, or <c>null</c> if no data is available.</returns>
        Task<string?> ReadLine();

        /// <summary>
        /// Writes a command to the engine's input stream.
        /// </summary>
        /// <param name="line">The command to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task WriteLine(string line);

        /// <summary>
        /// Validates whether a FEN string is in the correct format.
        /// </summary>
        /// <param name="fen">The FEN string to validate.</param>
        /// <returns><c>true</c> if the FEN is valid; otherwise, <c>false</c>.</returns>
        bool ValidateFen(string fen);

        /// <summary>
        /// Retrieves the principal variations (PV) for the current position.
        /// </summary>
        /// <returns>A task that resolves to a dictionary where keys are MultiPV indices and values are arrays of moves.</returns>
        Task<Dictionary<int, string[]>> GetPV();

        /// <summary>
        /// Releases all resources used by the engine and terminates the process.
        /// </summary>
        new void Dispose();
    }
}