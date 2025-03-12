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
        private CommandBuilder _commandBuilder { get; set; }
        private ResponseParser _responseParser { get; set; }
        private ChessPosition _curentPosition { get; set; }

        public EventHandler<MoveAnalysis> AnalysisUpdated;

        public StockfishEngine(string path)
        {
            _enginePath = path;

            _engineProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            _engineProcess.Start();
            _engineInput = _engineProcess.StandardInput;
            _engineOutput = _engineProcess.StandardOutput;

            _engineOptions = new EngineOptions(this);
            _commandBuilder = new CommandBuilder();
            _responseParser = new ResponseParser();
            _curentPosition = new ChessPosition();
            AnalysisUpdated = delegate { };
        }

        public void SendCommand(string command)
        {
            _engineInput.WriteLine(command);
            _engineInput.Flush();
        }

        public bool IsReady()
        {
            return true;
        }

        public void SetPosition(ChessPosition position)
        {
            _curentPosition = position;
        }

        public void StartAnalysis(int depth)
        {
            
        }

        public void StopAnalysis()
        {
            
        }

        public ChessMove GetBestMove(int timeInMs)
        {
            return null;
        }

        public void SetOption(string name, string value)
        {
            
        }
    }
}
