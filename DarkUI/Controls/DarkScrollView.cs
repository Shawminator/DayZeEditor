using System;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
	// Token: 0x02000044 RID: 68
	public abstract class DarkScrollView : DarkScrollBase
	{
		// Token: 0x0600030C RID: 780 RVA: 0x000130EC File Offset: 0x000112EC
		protected DarkScrollView()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
		}

		// Token: 0x0600030D RID: 781
		protected abstract void PaintContent(Graphics g);

		// Token: 0x0600030E RID: 782 RVA: 0x00013104 File Offset: 0x00011304
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			using (SolidBrush b = new SolidBrush(this.BackColor))
			{
				g.FillRectangle(b, base.ClientRectangle);
			}
			g.TranslateTransform((float)(base.Viewport.Left * -1), (float)(base.Viewport.Top * -1));
			this.PaintContent(g);
			g.TranslateTransform((float)base.Viewport.Left, (float)base.Viewport.Top);
			bool flag = this._vScrollBar.Visible && this._hScrollBar.Visible;
			if (flag)
			{
				using (SolidBrush b2 = new SolidBrush(this.BackColor))
				{
					Rectangle rect = new Rectangle(this._hScrollBar.Right, this._vScrollBar.Bottom, this._vScrollBar.Width, this._hScrollBar.Height);
					g.FillRectangle(b2, rect);
				}
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000043C9 File Offset: 0x000025C9
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}
	}
}
