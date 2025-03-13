using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sharpfish
{
    internal class EngineOptions
    {
        public EngineOptions()
        {
            // Set defaults
            Dictionary<string, string> options = new Dictionary<string, string>()
                {
                    { "Threads", "4" }, // Relatively low entry barrier, don't want to mess with detecting CPU
                    { "Hash", "256"}, // In MBs
                    { "MultiPV", "1"}, // Show only the best line
                    { "Skill Level", "20"}, // 0-20, default at 20
                };

            // Create command to send defaults to engine
            foreach (KeyValuePair<string, string> option in options)
            {
                CommandBuilder.SetOption(option.Key, option.Value);
            }
        }

        public void SetOption(string name,  string value)
        {
            CommandBuilder.SetOption(name, value);
        }
    }
}
