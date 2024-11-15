using System;
using System.Collections.Generic;

namespace E2Play
{
    public class TileLocationEventArgs : EventArgs
    {
        public int Row;
        public int Col;
        public List<int> PotentialPieces;
        public int Piece;
    }
}
