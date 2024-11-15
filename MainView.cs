using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace E2Play
{
    public partial class MainView : Form
    {

        bool PieceListLoaded = false;
        bool IsActive = false;
        int PlacedCount = 0;
        ListViewItem PreviousListViewItem = null;

        bool ProcessingItem = false;

        //string ProgressFile = "E2PlayProgress.e2p";
        readonly string ConfigFile = "E2Play.cfg";
        //string SaveInFolder;
        public MainView()
        {
            InitializeComponent();

        }

        private void MainLoad(object sender, EventArgs e)
        {
            //Show();
            LoadConfig();
            LoadBoard();
            board1.ShowClues = chkShowClues.Checked;
            if (chkShowClues.Checked)
                PlacedCount = 5;

            PieceList.SmallImageList=board1.GetCurrentImages();

            UpdatePlacedPieces();

            IsActive = true;
            Show();
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
                    chkSize.Checked = Convert.ToBoolean(icfg.ReadLine());
                }
            }
            AdjustSize(chkSize.Checked);
        }

        private void LoadBoard()
        {
            board1.LoadBoard(chkSize.Checked);

            //Setup event callbacks
            board1.TileSelected += TileSelected;
            board1.TileCleared += TileCleared;
            board1.PiecePlaced += PiecePlaced;

            UpdatePieceList(0, 0,new List<int> { });
        }

        void UpdatePlacedPieces()
        {
            lblPiecesPlaced.Text = $"{PlacedCount}/256";
        }

        private void TileSelected(object sender, TileLocationEventArgs e)
        {
            UpdatePieceList(e.Row, e.Col, e.PotentialPieces);
        }

        private void TileCleared(object sender, TileLocationEventArgs e)
        {
            UpdatePieceList(e.Row, e.Col, e.PotentialPieces);
            PlacedCount--;
            UpdatePlacedPieces();
        }

        private void UpdatePieceList(int Row, int Col, List<int> PossiblePieces)
        {
            PieceList.BeginUpdate();
            PieceListLoaded = false;
            //Now get a list of tiles that fit the selection.
            //We need to look at the all the surrounding tiles
            //We also need to cater for edges and corners


            List<int> result = board1.GetListOfMatchingPieces(Row, Col);
            PieceList.Items.Clear();
            colHeader.Text = $"Count: {result.Count}";
            foreach (int p in result)
            {
                PieceList.Items.Add(p.ToString(),p);
            }
            //PieceList.SelectedIndex = -1;
            PieceListLoaded = true;
            PieceList.EndUpdate();
        }

        private void PiecePlaced(object sender, TileLocationEventArgs e)
        {
            //UpdatePieceList(e.Row, e.Col, e.PotentialPieces);
        }

        private void chkShowClues_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsActive)
                return;

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
                UpdatePieceList(0, 0, new List<int> { });
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
                    //see if this is a backtracker save
                    if (OpenE2PFile.FileName.EndsWith("E2O"))
                    {
                        //this is basic file with pieces in rows and columns
                        using (StreamReader e2oFile = new StreamReader(OpenE2PFile.FileName))
                        {
                            string ldata = e2oFile.ReadLine();
                            string[] col;
                            int thepiecenumber;

                            if (Convert.ToInt32(ldata) != 16)
                            {
                                MessageBox.Show("Can only read 16x16 saves at the moment", "Invalid save file", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            else
                            {
                                PlacedCount = 0;
                                int[,] sp = new int[16, 16];
                                for (int r = 0; r < 16; r++)
                                {
                                    ldata = e2oFile.ReadLine();
                                    col = ldata.Split(new char[] { ',' });
                                    for (int c = 0; c < 16; c++)
                                    {
                                        thepiecenumber = Convert.ToInt32(col[c]);
                                        if (thepiecenumber == 1025 || thepiecenumber == 1026)
                                            thepiecenumber = 0;
                                        sp[r, c] = thepiecenumber;
                                        if (thepiecenumber != 0)
                                        {
                                            PlacedCount++;
                                            UpdatePlacedPieces();
                                        }

                                    }
                                }
                                board1.LoadBoard(chkSize.Checked);
                                board1.ShowClues = chkShowClues.Checked;
                                UpdatePieceList(0, 0, new List<int> { });
                                board1.SetPlacedPieces(sp);
                                board1.ResetBoard();
                            }

                        }
                    }
                    else
                    {
                        board1.LoadBoard(chkSize.Checked);
                        board1.ShowClues = chkShowClues.Checked;
                        UpdatePieceList(0, 0, new List<int> { });
                        Progress E2Progress = BinarySerialization.ReadFromBinaryFile<Progress>(OpenE2PFile.FileName);
                        chkShowClues.Checked = E2Progress.ShowClues;
                        board1.SetSelectionTile(E2Progress.SelectedTile);
                        board1.SetPlacedPieces(E2Progress.PlacedPieces);
                        chkPieceText.Checked = E2Progress.ShowPieceText;
                        board1.ResetBoard();
                    }


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
            if (IsActive)
            {
                AdjustSize(chkSize.Checked);

                //now tell board to refresh
                board1.BoardSizeChanged(chkSize.Checked);
                PieceList.SmallImageList = board1.GetCurrentImages();
                //PieceList.SetTileSize(chkSize.Checked ? 48 : 32);
                _ = board1.GetSelectedTile();

                //TODO: What
                //UpdatePieceList(sel[0], sel[1]);
                Invalidate();
            }

        }

        private void AdjustSize(bool IsBig)
        {
            if (IsBig)
            {
                board1.Width = 768;
                board1.Height = 768;
                Width = 1103;
                Height = 833;
                PieceList.Height = 768;
            }
            else
            {
                board1.Width = 512;
                board1.Height = 512;
                Width = 849;
                Height = 580;
                PieceList.Height = 512;
            }
            SaveConfig();
        }

        private void PieceList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            if (PreviousListViewItem != null)
            {
                PreviousListViewItem.BackColor = Color.White;
            }

            e.Item.BackColor = Color.LightBlue;
            PreviousListViewItem = e.Item;

        }

        private void PieceList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (ProcessingItem | !PieceListLoaded)
                return;
            ProcessingItem = true;
            PieceList.BeginUpdate();

            ListViewItem itm=e.Item;

            int[] SelectedSquare = board1.GetSelectedTile();

            if (board1.GetPieceAt(SelectedSquare[0], SelectedSquare[1]) <= 0)
            {
                //No piece at location so add this new piece
                board1.PlacePieceAt(Int32.Parse(itm.Text), SelectedSquare[0], SelectedSquare[1]);
                PlacedCount++;
                itm.Remove();
            }
            else
            {
                //There is already a piece here so add it back into the list
                int CurrentPieceUnderSelection = board1.GetPieceAt(SelectedSquare[0], SelectedSquare[1]);
                PieceList.Items.Add(CurrentPieceUnderSelection.ToString(), CurrentPieceUnderSelection);
                board1.PlacePieceAt(Int32.Parse(itm.Text), SelectedSquare[0], SelectedSquare[1]);
                itm.Remove();
            }
            //board1.PlacePiece(((PieceListItem)PieceList.Items[PieceList.SelectedIndex]).PieceNumber);

            Console.WriteLine($"Selected: {e.ItemIndex}");
            PieceList.EndUpdate();
            UpdatePlacedPieces();
            board1.UpdateSurrounds(SelectedSquare[0], SelectedSquare[1]);
            board1.Refresh();
            ProcessingItem = false;
        }
    }
}
