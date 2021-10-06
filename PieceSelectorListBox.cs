using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace E2Play
{
    [ToolboxItem(true)]
    class PieceSelectorListBox : ListBox
    {
        public PieceSelectorListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 50;
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.Items.Count > 0 && e.Index>=0)
            {
                base.OnDrawItem(e);
                PieceListItem PItem = (PieceListItem)this.Items[e.Index];

                e.DrawBackground();
                e.Graphics.DrawImage(PItem.PieceImage, new RectangleF(e.Bounds.Left, e.Bounds.Top, 48, 48), new RectangleF(0, 0, 48, 48), GraphicsUnit.Pixel);

                // Draw the text.
                e.Graphics.DrawString(PItem.PieceNumber.ToString(), this.Font, new SolidBrush(e.ForeColor), new RectangleF(e.Bounds.Left + 51, e.Bounds.Top + 16, e.Bounds.Right - 1 - (e.Bounds.Left + 51), e.Bounds.Bottom - 1 - (e.Bounds.Top + 16)));
                e.DrawFocusRectangle();
            }
        }
    }
}
