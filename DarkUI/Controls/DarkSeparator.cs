using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000046 RID: 70
	public class DarkSeparator : Control
	{
		// Token: 0x06000319 RID: 793 RVA: 0x0001355C File Offset: 0x0001175C
		public DarkSeparator()
		{
			base.SetStyle(ControlStyles.Selectable, false);
			this.Dock = DockStyle.Top;
			base.Size = new Size(1, 2);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0001358C File Offset: 0x0001178C
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			using (Pen p = new Pen(Colors.DarkBorder))
			{
				g.DrawLine(p, base.ClientRectangle.Left, 0, base.ClientRectangle.Right, 0);
			}
			using (Pen p2 = new Pen(Colors.LightBorder))
			{
				g.DrawLine(p2, base.ClientRectangle.Left, 1, base.ClientRectangle.Right, 1);
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000043C9 File Offset: 0x000025C9
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}
	}
}
