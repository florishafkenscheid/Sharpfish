using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal static class CommandBuilder
    {
        public static string NewGame() => "ucinewgame";
        public static string Position(string fen) => $"position fen {fen}";
        public static string Go() => $"go depth 20";
        public static string Go(int timeMs) => $"go movetime {timeMs}";
        public static string SetOption(string name, string value) => $"setoption name {name} value {value}";
        public static string IsReady() => "isready";
    }
}
