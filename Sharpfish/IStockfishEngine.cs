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
<<<<<<< HEAD
        Task<string> GetBestMove(int? timeMs = null, CancellationToken cancellationToken = default);
=======
        Task<string> GetBestMove(double? timeMs = null);
>>>>>>> 882d7c4 (Added .sln, made tests, fixed everything that needed to be fixed)
        Task SetOption(string key, string value);
        Task<bool> IsReady();
        Task<string> ReadUntil(params string[] expected);
        Task<string?> ReadLine();
        Task WriteLine(string line);
        bool ValidateFen(string fen);
        Task<Dictionary<int, string[]>> GetPV();
        new void Dispose();
    }
}
