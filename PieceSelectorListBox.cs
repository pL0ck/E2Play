using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace E2Play
{
    [ToolboxItem(true)]
    class PieceSelectorListBox : ListBox
    {
        private int TheTileSize = 32;
        string PText = "";
        string Rot = "";
        int ActualPiece = 0;

        public PieceSelectorListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = TheTileSize+2;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.Items.Count > 0 && e.Index>=0)
            {
                base.OnDrawItem(e);
                PieceListItem PItem = (PieceListItem)this.Items[e.Index];

                e.DrawBackground();
                e.Graphics.DrawImage(PItem.PieceImage, new RectangleF(e.Bounds.Left, e.Bounds.Top, TheTileSize, TheTileSize), new RectangleF(0, 0, TheTileSize, TheTileSize), GraphicsUnit.Pixel);

                ActualPiece = PItem.PieceNumber;

                if(ActualPiece >768)
                {
                    Rot = "/3";
                    ActualPiece -= 768;
                }
                else if(ActualPiece >512) 
                {
                    Rot = "/2";
                    ActualPiece -= 512;
                }
                else if(ActualPiece > 256)
                {
                    Rot = "/1";
                    ActualPiece -= 256;
                }
                else
                {
                    Rot = "/0";
                }
                PText = PItem.PieceNumber.ToString() + " " + ActualPiece.ToString()+Rot;
                // Draw the text.
                e.Graphics.DrawString(PText, this.Font, new SolidBrush(e.ForeColor), new RectangleF(e.Bounds.Left + 51, e.Bounds.Top + 16, e.Bounds.Right - 1 - (e.Bounds.Left + 51), e.Bounds.Bottom - 1 - (e.Bounds.Top + 16)));
                e.DrawFocusRectangle();
            }
        }

        public void SetTileSize(int TileSize)
        {
            TheTileSize = TileSize;
            ItemHeight = TileSize+2;
            Invalidate();
        }
    }
}
