using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal class MoveAnalysis
    {
        public ChessMove bestMove;
        public List<ChessMove> principalVariation;
        public int score;
        public bool isMateSeen;
        public int? mateInMoves;
        public int depth;
        public int nodes;
        public TimeSpan searchTime;
        public int nodesPerSecond;

        public MoveAnalysis()
        {
            bestMove = new ChessMove();
            principalVariation = new List<ChessMove>();
        }
    }
}
