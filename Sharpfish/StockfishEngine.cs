using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sharpfish.Sharpfish;

namespace Sharpfish
{
    public class StockfishEngine : IStockfishEngine
    {
        private readonly Process _process;
        private readonly StreamWriter _input;
        private readonly StreamReader _output;

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
                }
            };
            _process.Start();
            _input = _process.StandardInput;
            _output = _process.StandardOutput;

            // Options
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
        public async Task NewGame()
        {
            await WriteLine(CommandBuilder.NewGame());
            if (!await IsReady())
            {
                throw new Exception("Engine not ready when starting new game");
            }
        }

        public async Task setFenPosition(string fen)
        {
            if (!ValidateFen(fen)) throw new ArgumentException("Invalid FEN");

            await WriteLine(CommandBuilder.Position(fen));
        }

        public async Task<string> getEvaluation()
        {
            await WriteLine(CommandBuilder.Evaluate());
            string response = await ReadUntil("Final evaluation");
            return ResponseParser.ParseEvaluation(response);
        }

        public async Task<string> GetBestMove(int? timeMs = null, CancellationToken cancellationToken = default)
        {
            if (timeMs.HasValue)
            {
                await WriteLine(CommandBuilder.Go(timeMs.Value));
            }
            else
            {
                await WriteLine(CommandBuilder.Go());
            }

            string response = await ReadUntil("bestmove");
            return ResponseParser.ParseBestMove(response);
        }
        public async Task SetOption(string key, string value)
        {
            await WriteLine(CommandBuilder.SetOption(key, value));
        }
        public async Task<bool> IsReady()
        {
            await WriteLine(CommandBuilder.IsReady());
            string response = await ReadUntil("readyok");
            return ResponseParser.ParseReadyOK(response);
        }

        public async Task<string> ReadUntil(string expected)
        {
            Task? timeout = Task.Delay(TimeSpan.FromSeconds(2));
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

                if (line.Contains(expected))
                {
                    return line;
                }
            }
        }

        public async Task<string?> ReadLine()
        {
            return await _output.ReadLineAsync();
        }

        public async Task WriteLine(string line)
        {
            await _input.WriteLineAsync(line);
        }

        public bool ValidateFen(string fen)
        {
            // Regex: \s*^(((?:[rnbqkpRNBQKP1-8]+\/){7})[rnbqkpRNBQKP1-8]+)\s([b|w])\s(-|[K|Q|k|q]{1,4})\s(-|[a-h][1-8])\s(\d+\s\d+)$
            // From: https://gist.github.com/Dani4kor/e1e8b439115878f8c6dcf127a4ed5d3e
            string pattern = @"\s*^(((?:[rnbqkpRNBQKP1-8]+\/){7})[rnbqkpRNBQKP1-8]+)\s([b|w])\s(-|[K|Q|k|q]{1,4})\s(-|[a-h][1-8])\s(\d+\s\d+)$";

            if (Regex.IsMatch(fen, pattern))
            {
                MatchCollection regexList = Regex.Matches(fen, pattern);
                string[] splitFen = regexList[0].ToString().Split('/');

                if (splitFen.Length != 8)
                {
                    throw new Exception($"Expected 8 rows in position part of fen: {fen}");
                }

                foreach (string fenPart in splitFen)
                {
                    int sum = 0;
                    bool previous_was_digit = false;

                    foreach (char c in fenPart)
                    {
                        if ("12345678".Contains(c))
                        {
                            if (previous_was_digit)
                            {
                                throw new Exception($"Two subsequent digits in position part of fen: {fen}");
                            }

                            sum += c;
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

        public void Dispose()
        {
            _process.Close();
        }

        ~StockfishEngine()
        {
            Dispose();
        }
    }
}
