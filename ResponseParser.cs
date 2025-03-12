using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal class ResponseParser
    {
        private const string INFO_LINE = "info";
        private const string BESTMOVE_LINE = "bestmove";
        private const string READYOK = "readyok";
        private Dictionary<string, Action<string>> _parsers;

        public ResponseParser() 
        {
            _parsers = new Dictionary<string, Action<string>>();
        }

        public ChessMove ParseBestMove(string response)
        {
            return null;
        }

        public MoveAnalysis ParseMoveAnalysis(string response)
        {
            return null;
        }

        public EngineOptions ParseEngineOptions(string response)
        {
            return null;
        }

        public bool ParseReadyOk(string response)
        {
            return true;
        }
    }
}
