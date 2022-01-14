using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace E2Play
{
    public partial class MainView : Form
    {

        bool PieceListLoaded = false;
        bool IsActive = false;

        //string ProgressFile = "E2PlayProgress.e2p";
        readonly string ConfigFile = "E2Play.cfg";
        //string SaveInFolder;
        public MainView()
        {
            InitializeComponent();

        }

        private void MainLoad(object sender, EventArgs e)
        {
            Show();
            LoadConfig();

            if (chkSize.Checked)
                PieceList.SetTileSize(48);
            else
                PieceList.SetTileSize(32);

            LoadBoard();
            IsActive = true;
        }

        private void LoadConfig()
        {
            if (!File.Exists(ConfigFile))
            {
                using (StreamWriter cfg = new StreamWriter(ConfigFile))
                {
                    cfg.WriteLine(chkShowClues.Checked);
                    cfg.WriteLine(chkPieceText.Checked);
                    cfg.WriteLine(chkHideUselessPieces.Checked);
                    cfg.WriteLine(chkSize.Checked);
                }
            }
            else
            {
                using (StreamReader icfg = new StreamReader(ConfigFile))
                {
                    chkShowClues.Checked = Convert.ToBoolean(icfg.ReadLine());
                    chkPieceText.Checked = Convert.ToBoolean(icfg.ReadLine());
                    chkHideUselessPieces.Checked = Convert.ToBoolean(icfg.ReadLine());
                    chkSize.Checked=Convert.ToBoolean(icfg.ReadLine());
                }
            }
            AdjustSize(chkSize.Checked);
        }

        private void LoadBoard()
        {
            board1.LoadBoard(chkSize.Checked);
            board1.TileSelected += TileSelected;
            board1.TileCleared += TileCleared;
            board1.PiecePlaced += PiecePlaced;
            UpdatePieceList(0, 0);
        }

        private void TileSelected(object sender, TileLocationEventArgs e)
        {
            UpdatePieceList(e.Row, e.Col);
        }

        private void TileCleared(object sender, TileLocationEventArgs e)
        {
            UpdatePieceList(e.Row, e.Col);
        }

        private void UpdatePieceList(int Row, int Col)
        {
            PieceListLoaded = false;
            //Now get a list of tiles that fit the selection.
            //We need to look at the all the surrounding tiles
            //We also need to cater for edges and corners


            List<int> result = board1.GetListOfMatchingPieces(Row, Col);
            PieceList.Items.Clear();
            PieceCount.Text = $"Count: {result.Count}";
            foreach (int p in result)
            {
                PieceList.Items.Add(new PieceListItem(p, board1.GetPieceImage(p)));
            }
            PieceList.SelectedIndex = -1;
            PieceListLoaded = true;
        }

        private void PieceListSelected(object sender, EventArgs e)
        {
            if (PieceList.SelectedIndex == -1 || !PieceListLoaded)
                return;

            board1.PlacePiece(((PieceListItem)PieceList.Items[PieceList.SelectedIndex]).PieceNumber);
        }

        private void PiecePlaced(object sender, TileLocationEventArgs e)
        {
            UpdatePieceList(e.Row, e.Col);
        }

        private void chkShowClues_CheckedChanged(object sender, EventArgs e)
        {
            board1.ShowClues = chkShowClues.Checked;
            SaveConfig();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want reset and clear the board?", "Reset board", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                //Reset the board
                board1.LoadBoard(chkSize.Checked);
                board1.ShowClues = chkShowClues.Checked;
                UpdatePieceList(0, 0);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (SaveE2PFile.ShowDialog() != DialogResult.OK)
                return;

            Progress E2Progress = new Progress();
            E2Progress.ShowClues = chkShowClues.Checked;
            E2Progress.SelectedTile = board1.GetSelectedTile();
            E2Progress.PlacedPieces = board1.GetPlacedPieces();
            E2Progress.ShowPieceText = chkPieceText.Checked;

            BinarySerialization.WriteToBinaryFile<Progress>(SaveE2PFile.FileName, E2Progress);
            MessageBox.Show($"Progress Saved to {SaveE2PFile.FileName}", "Progress Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //ask user for file based on last save
            if (OpenE2PFile.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("Are you sure you want to load and overwrite board?", "Load board", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Progress E2Progress = BinarySerialization.ReadFromBinaryFile<Progress>(OpenE2PFile.FileName);
                    chkShowClues.Checked = E2Progress.ShowClues;
                    board1.SetSelectionTile(E2Progress.SelectedTile);
                    board1.SetPlacedPieces(E2Progress.PlacedPieces);
                    chkPieceText.Checked = E2Progress.ShowPieceText;
                    board1.ResetBoard();
                }
            }
        }

        private void MainViewClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        private void chkPieceText_CheckedChanged(object sender, EventArgs e)
        {
            board1.ShowPieceText = chkPieceText.Checked;
            SaveConfig();
            board1.Refresh();
        }

        private void chkHideUselessPieces_CheckedChanged(object sender, EventArgs e)
        {
            board1.HidePiecesWithZeroSurround = chkHideUselessPieces.Checked;
            SaveConfig();
        }

        private void SaveConfig()
        {
            try
            {
                using (StreamWriter cfg = new StreamWriter(ConfigFile))
                {
                    cfg.WriteLine(chkShowClues.Checked);
                    cfg.WriteLine(chkPieceText.Checked);
                    cfg.WriteLine(chkHideUselessPieces.Checked);
                    cfg.WriteLine(chkSize.Checked);
                }
            }
            catch
            {

            }

        }

        private void chkSize_CheckedChanged(object sender, EventArgs e)
        {
            if(IsActive)
            {
                AdjustSize(chkSize.Checked);

                //now tell board to refresh
                board1.BoardSizeChanged(chkSize.Checked);
                PieceList.SetTileSize(chkSize.Checked ? 48 : 32);
                int[] sel = board1.GetSelectedTile();

                UpdatePieceList(sel[0],sel[1]);
                Invalidate();
            }

        }

        private void AdjustSize(bool IsBig)
        {
            if(IsBig)
            {
                board1.Width = 768;
                board1.Height = 768;
                Width = 1067;
                Height = 833;
                PieceList.Height = 754;
            }
            else
            {
                board1.Width = 512;
                board1.Height = 512;
                Width = 813;
                Height = 580;
                PieceList.Height = 504;
            }
            SaveConfig();
        }
    }
}
