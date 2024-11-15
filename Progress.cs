using System;

namespace E2Play
{
    [Serializable]
    class Progress
    {
        public bool ShowClues { get; set; }
        public int[] SelectedTile { get; set; }
        public int[,] PlacedPieces { get; set; }
        public bool ShowPieceText { get; set; }
    }
}
