using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal class StockfishEngine
    {
        private Process _engineProcess { get; set; }
        private StreamWriter _engineInput { get; set; }
        private StreamReader _engineOutput { get; set; }
        private string _enginePath { get; set; }

        private bool _isInitialized { get; set; }
        private bool _isAnalyzing { get; set; }

        private EngineOptions _engineOptions { get; set; }
        private CommandBuilder _commandBuilder { get; set;  }
        private ResponseParser _responseParser { get; set;  }
        private ChessPosition _curentPosition { get; set; }

        private object _lockObject;

        public EventHandler<MoveAnalysis> AnalysisUpdated;

        public StockfishEngine(string path)
        {
            _enginePath = path;
            
            _engineProcess = Process.Start(path);
            _engineOptions = new EngineOptions();
            _commandBuilder = new CommandBuilder();
            _responseParser = new ResponseParser();


        }

        public void SendCommand(string command)
        {

        }

        public bool IsReady()
        {

        }

        public void SetPosition(ChessPosition position)
        {

        }

        public void StartAnalysis(int depth)
        {

        }

        public void StopAnalysis()
        {

        }

        public ChessMove GetBestMove(int timeInMs)
        {

        }

        public void SetOption(string name, string value)
        {

        }
    }
}
