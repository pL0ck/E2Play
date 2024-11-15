using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace E2Play
{
    [ToolboxItem(true)]
    public partial class Board : UserControl
    {
        struct TileSelector
        {
            public int Row;
            public int Col;
            public Pen SelectorPen;

        };

        struct SurroundingPieces
        {
            public int Row;
            public int Col;
            public int PieceCount;

        };
        struct PSearch
        {
            public int Top;
            public int Right;
            public int Bottom;
            public int Left;

            public void Clear()
            {
                Top = -1;
                Right = -1;
                Bottom = -1;
                Left = -1;
            }
        };
        
        //Stopwatch sw;

        Pieces E2Pieces;
        PSearch PieceSearch = new PSearch();

        int[,] PlacedPieces = new int[16, 16];
        int TileSize = 32;
        bool IsInitialised = false;
        TileSelector SelectedTile;
        TileSelector HighlightedTile;
        Pen SurroundPen;
        Font PieceFont;
        SolidBrush PieceBrush;
        Pen PieceFontBackground;

        //Store our images in an array
        private static Image[] AllPieces = new Image[1025];

        List<SurroundingPieces> HighlightSurround = new List<SurroundingPieces> { };

        public List<int> PossiblePieces { get; set; }

        public event EventHandler<TileLocationEventArgs> TileSelected;
        protected virtual void OnTileSelected(TileLocationEventArgs e)
        {
            TileSelected.Invoke(this, e);
        }

        public event EventHandler<TileLocationEventArgs> TileCleared;
        protected virtual void OnTileCleared(TileLocationEventArgs e)
        {
            TileCleared?.Invoke(this, e);
        }

        public event EventHandler<TileLocationEventArgs> PiecePlaced;
        protected virtual void OnPiecePlaced(TileLocationEventArgs e)
        {
            PiecePlaced?.Invoke(this, e);
        }

        bool CluesVisible = false;
        int PenSize = 3;

        public bool ShowClues
        {
            get
            {
                return CluesVisible;
            }
            set
            {
                CluesVisible = value;
                if (CluesVisible)
                {
                    SetCluePieces();
                }
                else
                {
                    ClearCluePieces();
                }
            }
        }
        public bool HidePiecesWithZeroSurround { get; set; }

        public bool ShowPieceText { get; set; }




        public Board()
        {
            InitializeComponent();
        }

        public void LoadBoard(bool IsBigBoard)
        {
            TileSize = (IsBigBoard) ? 48 : 32;
            //if (IsBigBoard)
            //    TileSize = 48;
            //else
            //    TileSize = 32;

            E2Pieces = new Pieces(TileSize);
            InitialiseImages();
            InitialiseBoard();
            InitialiseSelectors();
            IsInitialised = true;

            //Set the initial selection and highlight;
            HighlightedTile.Row = 0;
            HighlightedTile.Col = 0;
            SelectedTile.Row = HighlightedTile.Row;
            SelectedTile.Col = HighlightedTile.Col;

            //Get the initial list of possible pieces
            PossiblePieces = GetListOfMatchingPieces(SelectedTile.Row, SelectedTile.Col);
            UpdateSurrounds(SelectedTile.Row, SelectedTile.Col);

            Refresh();
        }

        private void InitialiseImages()
        {
            ImageList PieceImages = E2Pieces.GetAllImages();
            for (int i = 0; i < 1025; i++)
            {
                AllPieces[i] = PieceImages.Images[i];
            }
        }

        private void InitialiseBoard()
        {
            PossiblePieces = new List<int> { };
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    PlacedPieces[row, col] = 0;
                }
            }
            for (int i = 0; i < 1024; i++)
            {
                E2Pieces.ClearPieceInUse(i + 1);
            }
        }

        private void InitialiseSelectors()
        {
            SelectedTile.SelectorPen = new Pen(Color.Cyan, PenSize)
            {
                Alignment = System.Drawing.Drawing2D.PenAlignment.Inset,
                DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot
            };
            SelectedTile.Row = 0;
            SelectedTile.Col = 0;

            HighlightedTile.SelectorPen = new Pen(Color.White, PenSize)
            {
                Alignment = System.Drawing.Drawing2D.PenAlignment.Inset,
                DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
            };
            HighlightedTile.Row = 0;
            HighlightedTile.Col = 0;

            SurroundPen = new Pen(Color.Black, PenSize)
            {
                Alignment = System.Drawing.Drawing2D.PenAlignment.Inset,
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            //if(TileSize == 48)
            //    PieceFont = new Font("Tahoma", 8, FontStyle.Bold);
            //else
                PieceFont = new Font("Tahoma", 6, FontStyle.Bold);

            PieceBrush = new SolidBrush(Color.Black);
            PieceFontBackground = new Pen(new SolidBrush(Color.FromArgb(224, 255, 255, 255)));

        }

        public void BoardSizeChanged(bool IsBigBoard)
        {
            TileSize = (IsBigBoard) ? 48 : 32;
            E2Pieces.TileSizeChanged(IsBigBoard);
            InitialiseImages();

            //Now redisplay the board
            //and then the current piece layout
            Invalidate();


        }

        private string PieceToString(int PieceNumber)
        {
            //string result;

            //int ActualPiece = PieceNumber % 256;
            //result = ActualPiece.ToString();
            //int Rotation = PieceNumber / 256;
            //if (Rotation > 0)
            //    result = $"{result}/{Rotation}";


            //return result;


            return (PieceNumber / 256 > 0) ? $"{PieceNumber % 256}/{(int)(PieceNumber / 256)}" : $"{PieceNumber % 256}";
        }

        protected override void OnPaintBackground(PaintEventArgs pe)
        {
            //long tm = 0;
            if (!IsInitialised)
                return;
            string PieceText;

            //First we need to loop through the placed pieces and draw
            //sw=Stopwatch.StartNew();
            //tm=sw.ElapsedMilliseconds;
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    //Calculate the X & Y coord based on Row & Col
                    pe.Graphics.DrawImage(AllPieces[PlacedPieces[row, col]], col * TileSize, row * TileSize, TileSize, TileSize);
                    //Console.WriteLine("DrawImage {0}", sw.ElapsedMilliseconds);

                    if (PlacedPieces[row, col] > 0 && ShowPieceText)
                    {
                        PieceText = PieceToString(PlacedPieces[row, col]);
                        // Measure string.
                        SizeF TextSize = new SizeF();
                        TextSize = pe.Graphics.MeasureString(PieceText, PieceFont);

                        pe.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(224, 255, 255, 255)), col * TileSize + ((TileSize / 2) - (TextSize.Width / 2)), row * TileSize + ((TileSize / 2) - (TextSize.Height / 2)), TextSize.Width, TextSize.Height);
                        //Add the piece number
                        //First get the x & y for the rectangle
                        pe.Graphics.DrawString(PieceText, PieceFont, PieceBrush, col * TileSize + ((TileSize / 2) - (TextSize.Width / 2)), row * TileSize + ((TileSize / 2) - (TextSize.Height / 2)));

                    }

                }
            }
            //Console.WriteLine("OnPaint {0}",sw.ElapsedMilliseconds);
            //sw.Stop();

            //Make sure to draw our selection as well
            pe.Graphics.DrawRectangle(SelectedTile.SelectorPen, SelectedTile.Col * TileSize, SelectedTile.Row * TileSize, TileSize, TileSize);

            //and also our highlight
            pe.Graphics.DrawRectangle(HighlightedTile.SelectorPen, HighlightedTile.Col * TileSize, HighlightedTile.Row * TileSize, TileSize, TileSize);

            //Now look at our highlights and add those too
            if (HighlightSurround.Count > 0)
            {
                foreach (SurroundingPieces sp in HighlightSurround)
                {
                    //do different pens depending on number
                    //red will be 0
                    switch (sp.PieceCount)
                    {
                        case 0:
                            SurroundPen.Color = Color.FromArgb(255, 0, 0);
                            break;
                        case 1:
                            SurroundPen.Color = Color.FromArgb(255, 128, 128);
                            break;
                        case 2:
                            SurroundPen.Color = Color.FromArgb(255, 128, 0);
                            break;
                        case 3:
                            SurroundPen.Color = Color.FromArgb(192, 240, 32);
                            break;
                        default:
                            SurroundPen.Color = Color.FromArgb(0, 255, 0);
                            break;
                    }
                    pe.Graphics.DrawRectangle(SurroundPen, sp.Col * TileSize, sp.Row * TileSize, TileSize, TileSize);
                }
            }
        }

        public bool PieceHasZeroInSurround(int PieceNumber, int Row, int Col)
        {
            //First save off our current piece on the selected tile
            //Also any current highlight
            int TempPlacedPiece = PlacedPieces[Row, Col];
            //List<SurroundingPieces> TempHighlights = HighlightSurround;

            bool ZeroResult = false;
            //Now put our piece there and check surrounds
            PlacedPieces[Row, Col] = PieceNumber;
            UpdateSurrounds(Row, Col);

            HighlightSurround.ForEach(x => { if (x.PieceCount == 0) ZeroResult = true; });

            //HighlightSurround= TempHighlights;
            PlacedPieces[Row, Col] = TempPlacedPiece;
            return ZeroResult;
        }

        public void UpdateSurrounds(int Row, int Col)
        {
            //Lets look at all the empty spaces around this piece and see how many pieces are available
            HighlightSurround.Clear();
            List<int> MatchingPieces = new List<int> { };

            //Top Left corner
            if (Row == 0 && Col == 0)
            {
                if (PlacedPieces[Row, Col + 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col + 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col + 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row + 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row + 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row + 1, PieceCount = MatchingPieces.Count });
                }

            }

            //Top Right corner
            if (Row == 0 && Col == 15)
            {
                if (PlacedPieces[Row, Col - 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col - 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col - 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row + 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row + 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row + 1, PieceCount = MatchingPieces.Count });
                }

            }

            //Bottom Left corner
            if (Row == 15 && Col == 0)
            {
                if (PlacedPieces[Row, Col + 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col + 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col + 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row - 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row - 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row - 1, PieceCount = MatchingPieces.Count });
                }

            }

            //Bottom Right corner
            if (Row == 15 && Col == 15)
            {
                if (PlacedPieces[Row, Col - 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col - +1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col - 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row - 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row - 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row - 1, PieceCount = MatchingPieces.Count });
                }

            }

            //Top Edge
            if (Row == 0 && Col > 0 && Col < 15)
            {
                if (PlacedPieces[Row, Col + 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col + 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col + 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row, Col - 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col - 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col - 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row + 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row + 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row + 1, PieceCount = MatchingPieces.Count });
                }

            }

            //Bottom Edge
            if (Row == 15 && Col > 0 && Col < 15)
            {
                if (PlacedPieces[Row, Col + 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col + 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col + 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row, Col - 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col - 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col - 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row - 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row - 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row - 1, PieceCount = MatchingPieces.Count });
                }

            }

            //Left Edge
            if (Col == 0 && Row > 0 && Row < 15)
            {
                if (PlacedPieces[Row, Col + 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col + 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col + 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row - 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row - 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row - 1, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row + 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row + 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row + 1, PieceCount = MatchingPieces.Count });
                }

            }

            //Right Edge
            if (Col == 15 && Row > 0 && Row < 15)
            {
                if (PlacedPieces[Row, Col - 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col - 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col - 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row - 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row - 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row - 1, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row + 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row + 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row + 1, PieceCount = MatchingPieces.Count });
                }

            }

            //All Internal
            //Right Edge
            if (Col > 0 && Col < 15 && Row > 0 && Row < 15)
            {
                if (PlacedPieces[Row, Col - 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col - 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col - 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row, Col + 1] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row, Col + 1);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col + 1, Row = Row, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row - 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row - 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row - 1, PieceCount = MatchingPieces.Count });
                }
                if (PlacedPieces[Row + 1, Col] == 0)
                {
                    MatchingPieces = GetListOfMatchingPieces(Row + 1, Col);
                    HighlightSurround.Add(new SurroundingPieces { Col = Col, Row = Row + 1, PieceCount = MatchingPieces.Count });
                }

            }
        }

        private void BoardMouseClick(object sender, MouseEventArgs e)
        {
            //Make sure we cant select a clue if they are visible
            if (CluesVisible && PlacedPieces[HighlightedTile.Row, HighlightedTile.Col] != 0)
            {
                if (E2Pieces.IsClue(PlacedPieces[HighlightedTile.Row, HighlightedTile.Col]))
                    return;
            }

            //See if it's a left or right click
            //left adds the tile
            //Right removes
            if (e.Button == MouseButtons.Left)
            {
                //Update the new selection
                SelectedTile.Row = HighlightedTile.Row;
                SelectedTile.Col = HighlightedTile.Col;

                //Now get the pieces that can go here
                //PossiblePieces.Clear();
                PossiblePieces = GetListOfMatchingPieces(SelectedTile.Row, SelectedTile.Col);

                //Now go through and remove anything where it has a surround of 0
                if (HidePiecesWithZeroSurround)
                {
                    for (int i = PossiblePieces.Count - 1; i >= 0; i--)
                    {
                        if (PieceHasZeroInSurround(PossiblePieces[i], SelectedTile.Row, SelectedTile.Col))
                            PossiblePieces.RemoveAt(i);
                    }
                }

                UpdateSurrounds(SelectedTile.Row, SelectedTile.Col);

                TileLocationEventArgs SelectionData = new TileLocationEventArgs
                {
                    Row = SelectedTile.Row,
                    Col = SelectedTile.Col,
                    PotentialPieces=PossiblePieces
                };
                Refresh();
                OnTileSelected(SelectionData);
            }
            else if (e.Button == MouseButtons.Right)
            {
                //Remove tile
                //Make sure its not a blank already
                if (PlacedPieces[HighlightedTile.Row, HighlightedTile.Col] == 0)
                    return;

                //Get the tile and mark it not in use
                ClearPieceAt(HighlightedTile.Row, HighlightedTile.Col);

                PossiblePieces = GetListOfMatchingPieces(HighlightedTile.Row, HighlightedTile.Col);

                //Now go through and remove anything where it has a surround of 0
                if (HidePiecesWithZeroSurround)
                {
                    for (int i = PossiblePieces.Count - 1; i >= 0; i--)
                    {
                        if (PieceHasZeroInSurround(PossiblePieces[i], HighlightedTile.Row, HighlightedTile.Col))
                            PossiblePieces.RemoveAt(i);
                    }
                }

                UpdateSurrounds(SelectedTile.Row, SelectedTile.Col);

                TileLocationEventArgs SelectionData = new TileLocationEventArgs
                {
                    Row = SelectedTile.Row,
                    Col = SelectedTile.Col,
                    Piece = PlacedPieces[HighlightedTile.Row, HighlightedTile.Col]
                };
                Refresh();
                OnTileCleared(SelectionData);
            }
        }

        private void BoardMouseMove(object sender, MouseEventArgs e)
        {
            int MCol = e.X / TileSize;
            int MRow = e.Y / TileSize;
            if (MRow != HighlightedTile.Row || MCol != HighlightedTile.Col)
            {
                //Now set our new Highlighted location
                HighlightedTile.Row = MRow;
                HighlightedTile.Col = MCol;
                Refresh();
            }

        }

        private void BoardPanel_MouseLeave(object sender, EventArgs e)
        {
            Refresh();
        }

        public int GetTopAt(int Row, int Col)
        {
            return PlacedPieces[Row, Col] != 0 ? E2Pieces.GetTop(PlacedPieces[Row, Col]) : -1;
        }

        public int GetRightAt(int Row, int Col)
        {
            return PlacedPieces[Row, Col] != 0 ? E2Pieces.GetRight(PlacedPieces[Row, Col]) : -1;
        }

        public int GetBottomAt(int Row, int Col)
        {
            return PlacedPieces[Row, Col] != 0 ? E2Pieces.GetBottom(PlacedPieces[Row, Col]) : -1;
        }

        public int GetLeftAt(int Row, int Col)
        {
            return PlacedPieces[Row, Col] != 0 ? E2Pieces.GetLeft(PlacedPieces[Row, Col]) : -1;
        }

        public Image GetPieceImage(int PieceNumber)
        {
            return E2Pieces.GetPiece(PieceNumber);
        }

        public List<int> GetListOfMatchingPieces(int Row, int Col)
        {
            PieceSearch.Clear();
            if (Row < 0 || Row > 15 || Col < 0 || Col > 15)
                return new List<int> { };

            if (Row == 0)
            {
                if (Col == 0)
                {
                    //This is the top left corner
                    PieceSearch.Top = 0;
                    PieceSearch.Left = 0;
                    PieceSearch.Right = GetLeftAt(Row, Col + 1);
                    PieceSearch.Bottom = GetTopAt(Row + 1, Col);
                }
                else if (Col == 15)
                {
                    //Top right corner
                    PieceSearch.Top = 0;
                    PieceSearch.Right = 0;
                    PieceSearch.Left = GetRightAt(Row, Col - 1);
                    PieceSearch.Bottom = GetTopAt(Row + 1, Col);
                }
            }

            if (Row == 15)
            {
                if (Col == 0)
                {
                    //Bottom left corner
                    PieceSearch.Left = 0;
                    PieceSearch.Bottom = 0;
                    PieceSearch.Right = GetLeftAt(Row, Col + 1);
                    PieceSearch.Top = GetBottomAt(Row - 1, Col);

                }
                else if (Col == 15)
                {
                    //Bottom right corner
                    PieceSearch.Right = 0;
                    PieceSearch.Bottom = 0;
                    PieceSearch.Left = GetRightAt(Row, Col - 1);
                    PieceSearch.Top = GetBottomAt(Row - 1, Col);
                }

            }

            //First see if this is an edge
            if (Row == 0 && Col > 0 && Col < 15)
            {
                //We have a top edge
                PieceSearch.Top = 0;

                //look at pieces left right bottom
                PieceSearch.Left = GetRightAt(Row, Col - 1);
                PieceSearch.Right = GetLeftAt(Row, Col + 1);
                PieceSearch.Bottom = GetTopAt(Row + 1, Col);
            }

            if (Row == 15 && Col > 0 && Col < 15)
            {
                //We have a bottom edge
                PieceSearch.Bottom = 0;

                //look at pieces left right top
                PieceSearch.Left = GetRightAt(Row, Col - 1);
                PieceSearch.Right = GetLeftAt(Row, Col + 1);
                PieceSearch.Top = GetBottomAt(Row - 1, Col);
            }

            if (Col == 0 && Row > 0 && Row < 15)
            {
                //We have a Left edge
                PieceSearch.Left = 0;

                //look at pieces top right bottom
                PieceSearch.Right = GetLeftAt(Row, Col + 1);
                PieceSearch.Top = GetBottomAt(Row - 1, Col);
                PieceSearch.Bottom = GetTopAt(Row + 1, Col);
            }

            if (Col == 15 && Row > 0 && Row < 15)
            {
                //We have a right edge
                PieceSearch.Right = 0;

                //look at pieces top left bottom
                PieceSearch.Top = GetBottomAt(Row - 1, Col);
                PieceSearch.Left = GetRightAt(Row, Col - 1);
                PieceSearch.Bottom = GetTopAt(Row + 1, Col);
            }

            if (Row > 0 && Row < 15 && Col > 0 && Col < 15)
            {
                //internal piece. look everywhere
                PieceSearch.Top = GetBottomAt(Row - 1, Col);
                PieceSearch.Left = GetRightAt(Row, Col - 1);
                PieceSearch.Bottom = GetTopAt(Row + 1, Col);
                PieceSearch.Right = GetLeftAt(Row, Col + 1);
            }

            return E2Pieces.GetMatchingPieces(PieceSearch.Top, PieceSearch.Right, PieceSearch.Bottom, PieceSearch.Left);
        }


        public void PlacePiece(int PieceNumber)
        {
            //First see if we have a piece there aleady
            //If we do we need to clear its use
            if (PlacedPieces[SelectedTile.Row, SelectedTile.Col] != 0)
                E2Pieces.ClearPieceInUse(PlacedPieces[SelectedTile.Row, SelectedTile.Col]);

            PlacedPieces[SelectedTile.Row, SelectedTile.Col] = PieceNumber;
            E2Pieces.SetPieceInUse(PieceNumber);

            UpdateSurrounds(SelectedTile.Row, SelectedTile.Col);

            TileLocationEventArgs SelectionData = new TileLocationEventArgs
            {
                Row = SelectedTile.Row,
                Col = SelectedTile.Col,
                Piece = PieceNumber
            };

            Refresh();
            OnPiecePlaced(SelectionData);
        }

        public int GetPieceAt(int Row, int Col)
        {
            //int PieceNumber = -1;
            //if (Row >= 0 && Row < 16 && Col >= 0 && Col < 16)
            //{
            //    //Have a valid board position
            //    PieceNumber = PlacedPieces[Row, Col];
            //}
            return (Row >= 0 && Row < 16 && Col >= 0 && Col < 16) ? PlacedPieces[Row, Col] : -1;

        }

        public void PlacePieceAt(int PieceNumber, int Row, int Col)
        {
            PlacedPieces[Row, Col] = PieceNumber;
            E2Pieces.SetPieceInUse(PieceNumber);
            Refresh();
        }

        public void ClearPieceAt(int Row, int Col)
        {
            //First get the current piece at the location
            int PieceNumber = PlacedPieces[Row, Col];

            //If its already 0 then exit
            if (PieceNumber == 0)
                return;

            PlacedPieces[Row, Col] = 0;
            E2Pieces.ClearPieceInUse(PieceNumber);
            Refresh();
        }

        public int[] GetSelectedTile()
        {
            return new int[2] { SelectedTile.Row, SelectedTile.Col };
        }

        public void SetSelectionTile(int[] RowCol)
        {
            SelectedTile.Row = RowCol[0];
            SelectedTile.Col = RowCol[1];
        }

        public int[,] GetPlacedPieces()
        {
            return PlacedPieces;
        }

        public void SetPlacedPieces(int[,] pp)
        {
            for (int r = 0; r < 16; r++)
            {
                for (int c = 0; c < 16; c++)
                {
                    PlacedPieces[r, c] = pp[r, c];
                    if (pp[r, c] != 0)
                        E2Pieces.SetPieceInUse(pp[r, c]);
                }
            }
        }
        public void ResetBoard()
        {
            Refresh();
        }

        private void SetCluePieces()
        {
            PlacePieceAt(651, 8, 7);
            PlacePieceAt(976, 2, 2);
            PlacePieceAt(1023, 2, 13);
            PlacePieceAt(949, 13, 2);
            PlacePieceAt(249, 13, 13);
            Refresh();
        }

        private void ClearCluePieces()
        {
            //Clear only if clues are already placed
            if (PlacedPieces[8, 7] == 651)
                PlacedPieces[8, 7] = 0;
            if (PlacedPieces[2, 2] == 976)
                PlacedPieces[2, 2] = 0;
            if (PlacedPieces[2, 13] == 1023)
                PlacedPieces[2, 13] = 0;
            if (PlacedPieces[13, 2] == 949)
                PlacedPieces[13, 2] = 0;
            if (PlacedPieces[13, 13] == 249)
                PlacedPieces[13, 13] = 0;
            Refresh();
        }

        public ImageList GetCurrentImages()
        {
            return E2Pieces.GetAllImages();
        }
    }
}
