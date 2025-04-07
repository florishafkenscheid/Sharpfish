using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    /// <summary>
    /// Provides methods to build UCI (Universal Chess Interface) commands for interacting with a chess engine.
    /// </summary>
    public static class CommandBuilder
    {
        /// <summary>
        /// Generates a command to start a new game.
        /// </summary>
        /// <returns>The "ucinewgame" UCI command.</returns>
        public static string NewGame() => "ucinewgame";

        /// <summary>
        /// Generates a command to set the position using a FEN (Forsyth–Edwards Notation) string.
        /// </summary>
        /// <param name="fen">The FEN string describing the chess position.</param>
        /// <returns>The "position fen {fen}" UCI command.</returns>
        public static string Position(string fen) => $"position fen {fen}";

        /// <summary>
        /// Generates a command to set the position from the starting position and apply a sequence of moves.
        /// </summary>
        /// <param name="moves">An array of moves in UCI format (e.g., ["e2e4", "e7e5"]).</param>
        /// <returns>The "position startpos moves {moves}" UCI command.</returns>
        public static string Position(string[] moves) => $"position startpos moves {string.Join(" ", moves)}";

        /// <summary>
        /// Generates a command to start searching for the best move up to a specified depth.
        /// </summary>
        /// <param name="Depth">The depth of the search (number of plies).</param>
        /// <returns>The "go depth {Depth}" UCI command.</returns>
        public static string Go(int Depth) => $"go depth {Depth}";

        /// <summary>
        /// Generates a command to start searching for the best move within a specified time limit.
        /// </summary>
        /// <param name="timeMs">The maximum time allowed for the search, in milliseconds.</param>
        /// <returns>The "go movetime {timeMs}" UCI command.</returns>
        public static string Go(double timeMs) => $"go movetime {timeMs}";

        /// <summary>
        /// Generates a command to set a UCI option.
        /// </summary>
        /// <param name="name">The name of the option to set.</param>
        /// <param name="value">The value to assign to the option.</param>
        /// <returns>The "setoption name {name} value {value}" UCI command.</returns>
        public static string SetOption(string name, string value) => $"setoption name {name} value {value}";

        /// <summary>
        /// Generates a command to check if the engine is ready.
        /// </summary>
        /// <returns>The "isready" UCI command.</returns>
        public static string IsReady() => "isready";

        /// <summary>
        /// Generates a command to request an evaluation of the current position.
        /// </summary>
        /// <returns>The "eval" UCI command.</returns>
        public static string Evaluate() => "eval";
    }
}