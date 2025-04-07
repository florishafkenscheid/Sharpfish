using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using Sharpfish;

namespace Sharpfish
{
    /// <summary>
    /// Provides an implementation of <see cref="IStockfishEngine"/> to communicate with the Stockfish chess engine.
    /// </summary>
    public class StockfishEngine : IStockfishEngine
    {
        public int Depth { get;  set; }
        public int MultiPV { get;  set; }

        private readonly Process _process;
        private readonly StreamWriter _input;
        private readonly StreamReader _output;
        private bool _disposed = false; // Prevent chance of double disposal

        /// <summary>
        /// Initializes a new instance of the Stockfish engine process.
        /// </summary>
        /// <param name="enginePath">The file path to the Stockfish executable.</param>
        /// <exception cref="FileNotFoundException">Thrown if the engine path is invalid.</exception>
        public StockfishEngine(string enginePath)
        {
            // Stockfish Process
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = enginePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                }
            };
            _process.Start();
            _input = _process.StandardInput;
            _output = _process.StandardOutput;

            // Options
            Depth = 20;

            Dictionary<string, string> options = new Dictionary<string, string>()
            {
                { "Threads", "4" }, // Relatively low entry barrier, don't want to mess with detecting CPU
                { "Hash", "256"}, // In MBs
                { "MultiPV", "1"}, // Show only the best line
                { "Skill Level", "20"}, // 0-20, default at 20
            };

            foreach (KeyValuePair<string, string> option in options)
            {
                _ = SetOption(option.Key, option.Value); // Needs a discard _ variable for some async reason
            }
        }

        /// <inheritdoc/>
        public async Task NewGame()
        {
            await WriteLine(CommandBuilder.NewGame());
        }

        /// <inheritdoc/>
        public async Task SetPosition(string fen)
        {
            if (!ValidateFen(fen)) throw new ArgumentException("Invalid FEN");

            await WriteLine(CommandBuilder.Position(fen));
        }

        /// <inheritdoc/>
        public async Task SetPosition(string[] moves)
        {
            await WriteLine(CommandBuilder.Position(moves));
        }

        /// <inheritdoc/>
        public async Task<string> GetEvaluation()
        {
            await WriteLine(CommandBuilder.Evaluate());
            string response = await ReadUntil("Final evaluation");
            return ResponseParser.ParseEvaluation(response);
        }

        /// <inheritdoc/>
        public async Task<string> GetBestMove(double? timeMs = null)
        {
            if (timeMs.HasValue)
            {
                await WriteLine(CommandBuilder.Go(timeMs.Value));
            }
            else
            {
                await WriteLine(CommandBuilder.Go(Depth));
            }

            string response = await ReadUntil("bestmove");
            return ResponseParser.ParseBestMove(response);
        }

        /// <inheritdoc/>
        public async Task SetOption(string key, string value)
        {
            if (key == "MultiPV")
            {
                MultiPV = int.Parse(value);
            }

            await WriteLine(CommandBuilder.SetOption(key, value));
        }

        /// <inheritdoc/>
        public async Task<bool> IsReady()
        {
            await WriteLine(CommandBuilder.IsReady());
            var response = await ReadUntil("readyok");
            return ResponseParser.ParseReadyOK(response);
        }

        /// <inheritdoc/>
        public async Task<string> ReadUntil(params string[] expected)
        {
            Task timeout = Task.Delay(TimeSpan.FromSeconds(100));
            while (true)
            {
                var lineTask = ReadLine();
                var completedTask = await Task.WhenAny(lineTask, timeout);

                if (completedTask == timeout)
                {
                    throw new TimeoutException("Engine response timeout");
                }

                var line = await lineTask;
                if (line == null)
                {
                    throw new InvalidOperationException("No output from engine stream");
                }

                bool allValuesFound = expected.All(value => line.Contains(value));
                if (allValuesFound)
                {
                    return line;
                }
            }
        }

        /// <inheritdoc/>
        public async Task<string?> ReadLine()
        {
            return await _output.ReadLineAsync();
        }

        /// <inheritdoc/>
        public async Task WriteLine(string line)
        {
            await _input.WriteLineAsync(line);
        }

        /// <inheritdoc/>
        public bool ValidateFen(string fen)
        {
            // Regex: \s*^(((?:[rnbqkpRNBQKP1-8]+\/){7})[rnbqkpRNBQKP1-8]+)\s([b|w])\s(-|[K|Q|k|q]{1,4})\s(-|[a-h][1-8])\s(\d+\s\d+)$
            // From: https://gist.github.com/Dani4kor/e1e8b439115878f8c6dcf127a4ed5d3e
            string pattern = @"\s*^(((?:[rnbqkpRNBQKP1-8]+\/){7})[rnbqkpRNBQKP1-8]+)\s([b|w])\s(-|[K|Q|k|q]{1,4})\s(-|[a-h][1-8])\s(\d+\s\d+)$";

            if (Regex.IsMatch(fen, pattern))
            {
                MatchCollection regexList = Regex.Matches(fen, pattern);
                string[] splitFen = regexList[0].ToString().Split('/', ' ');
                // rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
                // Into [rnbqkbnr, pppppppp, 8, 8, 8, 8, PPPPPPPP, RNBQKBNR, w, KQkq, -, 0, 1]

                if (splitFen.Length != 13)
                {
                    throw new Exception($"Expected 13 rows in fen: {fen}");
                }

                for (int i = 0; i < 8; i++) // 8 = splitFen[0-7], so stops before player to move indicator (w|b)
                {
                    int sum = 0;
                    bool previous_was_digit = false;

                    foreach (char c in splitFen[i])
                    {
                        if ("12345678".Contains(c))
                        {
                            if (previous_was_digit)
                            {
                                throw new Exception($"Two subsequent digits in position part of fen: {fen}");
                            }

                            sum += int.Parse(c.ToString());
                            previous_was_digit = true;
                        }
                        else if ("pnbrqk".Contains(char.ToLower(c)))
                        {
                            sum += 1;
                            previous_was_digit = false;
                        }
                        else
                        {
                            throw new Exception($"Invalid character in position part of fen: {fen}");
                        }
                    }

                    if (sum != 8)
                    {
                        throw new Exception($"Expected 8 columns per row in position part of fen: {fen}");
                    }
                }

                return true;
            }
            else
            {
                throw new Exception("Fen doesn`t match follow this example: rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 ");
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<int, string[]>> GetPV()
        {
            Dictionary<int, string[]> pv = [];
            
            await WriteLine(CommandBuilder.Go(Depth));

            for (int i = 1; i <= MultiPV; i++)
            {
                string response = await ReadUntil($"info depth {Depth}", $"multipv {i}");
                pv[i] = ResponseParser.ParsePV(response);
            }
            return pv;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _input?.Dispose();
                    _output?.Dispose();

                    // Terminate before dispose
                    if (_process != null && !_process.HasExited)
                    {
                        _process.Kill();
                    }
                    _process?.Dispose();
                }

                _disposed = true;
            }
        }

        ~StockfishEngine()
        {
            Dispose(false);
        }
    }
}
