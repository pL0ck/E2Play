using System;
using System.Collections.Generic;
using System.Text;

namespace E2Play
{
    [Serializable]
    class Progress
    {
        public bool ShowClues { get; set; }
        public int[] SelectedTile { get; set; }
        public int[,] PlacedPieces { get; set; }

    }
}
