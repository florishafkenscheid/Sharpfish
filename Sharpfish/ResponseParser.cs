using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    /// <summary>
    /// Provides methods to parse responses from the Stockfish chess engine.
    /// </summary>
    public static class ResponseParser
    {
        /// <summary>
        /// Determines if the engine's response indicates readiness.
        /// </summary>
        /// <param name="response">The raw response string from the engine.</param>
        /// <returns><c>true</c> if the response contains "readyok"; otherwise, <c>false</c>.</returns>
        public static bool ParseReadyOK(string response) => response?.Contains("readyok") == true;

        /// <summary>
        /// Extracts the best move from the engine's response.
        /// </summary>
        /// <param name="response">The raw response string containing "bestmove".</param>
        /// <returns>The best move in UCI format.</returns>
        /// <exception cref="InvalidDataException">Thrown if the response format is invalid.</exception>
        /// <exception cref="Exception">Thrown if no valid move is found.</exception>
        public static string ParseBestMove(string response)
        {
            ArgumentNullException.ThrowIfNull(response);

            if (response.StartsWith("bestmove "))
            {
                string[] parts = response.Split(' ');

                if (parts.Length < 2)
                    throw new InvalidDataException("Invalid bestmove format");

                if (parts[1] == "(none)")
                {
                    throw new Exception("No valid best move was found");
                }

                return parts[1];
            }

            throw new InvalidDataException("Response does not contain a bestmove");
        }

        /// <summary>
        /// Extracts the evaluation score from the engine's response.
        /// </summary>
        /// <param name="response">The raw response string containing evaluation data.</param>
        /// <returns>The evaluation score as a string.</returns>
        /// <exception cref="InvalidDataException">Thrown if the response format is invalid.</exception>
        public static string ParseEvaluation(string response)
        {
            try
            {
                return response.Split([' '], StringSplitOptions.RemoveEmptyEntries)[2];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidDataException("Invalid evaluation format: not enough parts in the response");
            }
        }

        /// <summary>
        /// Extracts the principal variation (PV) moves from an "info" response line.
        /// </summary>
        /// <param name="response">The raw "info" response line from the engine.</param>
        /// <returns>An array of moves in the principal variation.</returns>
        /// <remarks>Skips the first 21 elements of the "info" line to isolate the moves.</remarks>
        public static string[] ParsePV(string response)
        {
            ArgumentNullException.ThrowIfNull(response);

            if (response.StartsWith("info"))
            {
                return response
                    .Split(' ')
                    .Skip(21) // Skip the first 21 elements (info part)
                    .ToArray();
            }
            else
            {
                return [];
            }
        }
    }
}
