using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharpfish
{
    public interface IStockfishEngine : IDisposable
    {
        int Depth { get; set; }
        int MultiPV { get; set; }
        Task NewGame();
        Task SetPosition(string fen);
        Task SetPosition(string[] moves);
        Task<string> GetEvaluation();
        Task<string> GetBestMove(int? timeMs = null, CancellationToken cancellationToken = default);
        Task SetOption(string key, string value);
        Task<bool> IsReady();
        Task<string> ReadUntil(string expected);
        Task<string?> ReadLine();
        Task WriteLine(string line);
        bool ValidateFen(string fen);
        new void Dispose();
    }
}
