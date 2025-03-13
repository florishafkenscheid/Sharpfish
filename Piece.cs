using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    public abstract class Piece
    {
        private string _position;
        private readonly bool _isWhite;

        protected Piece(string type, string position, bool isWhite)
        {
            _position = position;
            _isWhite = isWhite;
        }

        public string Position
        {
            get => _position;
            set => _position = value;
        }

        public bool IsWhite => _isWhite;

        public abstract string Type { get; }
        public abstract char Symbol { get; }
    }
}
