using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal class CommandBuilder
    {
        private const string UCI_NEW_GAME = "ucinewgame";
        private const string POSITION = "position";
        private const string GO = "go";
        private const string STOP = "stop";
        private const string SET_OPTION = "setoption name";
        private const string IS_READY = "isready";
        private const string QUIT = "quit";

        public CommandBuilder() { }

        public string BuildPositionCommand(ChessPosition position) { }

        public string BuildGoCommand(int depth, int timeMs) { }
        public string BuildSetOptionCommand(string name, string value) { }
        public string BuildUciNewGameCommand() { }
        public string BuildStopCommand() { }
    }
}
