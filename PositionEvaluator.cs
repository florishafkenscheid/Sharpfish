using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal class PositionEvaluator
    {
        private StockfishEngine _engine { get; set; }
        private int _defaultDepth = 20;
        private int _defaultTimeMs = 100;
        private bool _useMultiPV = false;
        private int _multiPVCount = 3;
        private Dictionary<string, MoveAnalysis> _cachedEvaluations;

        public PositionEvaluator(StockfishEngine engine)
        {
            _engine = engine;
            _cachedEvaluations = new Dictionary<string, MoveAnalysis>();
        }

        public double EvaluateStatic(ChessPosition position) { }
        public double EvaluateAtDepth(ChessPosition position, int depth) { }
        public double EvaluateWithTime(ChessPosition position, int timeMs) { }
        public ChessMove[] GetTopMoves(ChessPosition position, int count) { }
    }
}
