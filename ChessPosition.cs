using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal class ChessPosition
    {
        private string _fenString;
        private Piece[,] _board;
        private bool _whiteToMove;
        private bool _whiteKingsideCastle;
        private bool _whiteQueensideCastle;
        private bool _blackKingsideCastle;
        private bool _blackQueensideCastle;
        private string _enPassantSquare;
        private int _halfMoveClock;
        private int _fullMoveNumber;

        public ChessPosition(string fenString)
        {
            _fenString = fenString;
            _board = ParseFen(_fenString);
            _enPassantSquare = string.Empty;
        }

        public Piece GetPieceAt(int rank, int file) 
        { 
            return _board[rank, file]; 
        }

        public void ApplyMove(ChessPosition position, ChessMove move) { }
        public bool IsValidMove(ChessPosition position, ChessMove move) { }
        public string ToUciString() { }

        public Piece[,] ParseFen(string fenSting) { }
    }
}
