using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal class EngineOptions
    {
        private Dictionary<string, string> _options { get; set; }
        private int _skillLevel { get; set; }
        private int _threads { get; set; }
        private int _hashSize { get; set; }
        private int _multiPV { get; set; }
        private bool _useNNUE { get; set; }
        private StockfishEngine _engine { get; set; }


        public EngineOptions(StockfishEngine engine)
        {
            _engine = engine;

            // Set defaults
            _options = new Dictionary<string, string>()
                {
                    { "Threads", "4" }, // Relatively low entry barrier, don't want to mess with detecting CPU
                    { "Hash", "256"}, // In MBs
                    { "MultiPV", "1"}, // Show only the best line
                    { "Skill Level", "20"}, // 0-20, default at 20
                };

            // Create command to send defaults to engine
            CommandBuilder commandBuilder = new CommandBuilder();
            string command = string.Empty; // TODO
            engine.SendCommand(command);
        }
    }
}
