using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    public static class CommandBuilder
    {
        public static string NewGame() => "ucinewgame";
        public static string Position(string fen) => $"position fen {fen}";
        public static string Position(string[] moves) => $"position startpos moves {string.Join(" ", moves)}";
        public static string Go(int Depth) => $"go depth {Depth}";
        public static string Go(double timeMs) => $"go movetime {timeMs}";
        public static string SetOption(string name, string value) => $"setoption name {name} value {value}";
        public static string IsReady() => "isready";
        public static string Evaluate() => "eval";
    }
}
