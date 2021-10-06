using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace E2Play
{
    public partial class MainView : Form
    {

        bool PieceListLoaded = false;
        string ProgressFile = "E2PlayProgress2.e2p";
        string ConfigFile = "E2Play.cfg";
        public MainView()
        {
            InitializeComponent();

        }

        private void MainLoad(object sender, EventArgs e)
        {
            Show();
            LoadBoard();
            LoadConfig();
        }

        private void LoadConfig()
        {
            if(!File.Exists(ConfigFile))
            {
                using(StreamWriter cfg=new StreamWriter(ConfigFile))
                {
                    cfg.WriteLine(chkShowClues.Checked);
                }
            }
            else
            {
                using (StreamReader icfg = new StreamReader(ConfigFile))
                {
                    chkShowClues.Checked = Convert.ToBoolean(icfg.ReadLine());
                }
            }
        }

        private void LoadBoard()
        {
            board1.LoadBoard();
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


            List<int> result = board1.GetListOfMatchingPieces(Row,Col);
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
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //Reset the board
            board1.LoadBoard();
            board1.ShowClues = chkShowClues.Checked;
            UpdatePieceList(0, 0);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            Progress E2Progress = new Progress();
            E2Progress.ShowClues = chkShowClues.Checked;
            E2Progress.SelectedTile = board1.GetSelectedTile();
            E2Progress.PlacedPieces = board1.GetPlacedPieces();

            BinarySerialization.WriteToBinaryFile<Progress>(ProgressFile, E2Progress);
            MessageBox.Show($"Progress Saved to {ProgressFile}", "Progress Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if(!File.Exists(ProgressFile))
            {
                MessageBox.Show($"File {ProgressFile} not found.\nYou need to save progress first before loading", "No Save found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(MessageBox.Show("Are you sure you want to load and overwrite board?","Load board",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Progress E2Progress = BinarySerialization.ReadFromBinaryFile<Progress>(ProgressFile);
                chkShowClues.Checked = E2Progress.ShowClues;
                board1.SetSelectionTile(E2Progress.SelectedTile);
                board1.SetPlacedPieces(E2Progress.PlacedPieces);
                board1.ResetBoard();
            }                
        }

        private void MainViewClosing(object sender, FormClosingEventArgs e)
        {
            using (StreamWriter cfg = new StreamWriter(ConfigFile))
            {
                cfg.WriteLine(chkShowClues.Checked);
            }
        }
    }
}
