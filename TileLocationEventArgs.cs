using System;
using System.Collections.Generic;
using System.Text;

namespace E2Play
{
    public class TileLocationEventArgs : EventArgs
    {
        public int Row;
        public int Col;
        public int Piece;
    }
}
