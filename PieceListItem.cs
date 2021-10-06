using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace E2Play
{

    class PieceListItem
    {
        public int PieceNumber;
        public Image PieceImage;

        public PieceListItem(int PNumber, Image PImage)
        {
            PieceNumber = PNumber;
            PieceImage = PImage;
        }
    }
}
