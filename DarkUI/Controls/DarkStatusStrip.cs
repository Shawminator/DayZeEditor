using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000047 RID: 71
	public class DarkStatusStrip : StatusStrip
	{
		// Token: 0x0600031C RID: 796 RVA: 0x00013640 File Offset: 0x00011840
		public DarkStatusStrip()
		{
			this.AutoSize = false;
			base.BackColor = Colors.GreyBackground;
			base.ForeColor = Colors.LightText;
			base.Padding = new Padding(0, 5, 0, 3);
			base.Size = new Size(base.Size.Width, 24);
			base.SizingGrip = false;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000136AC File Offset: 0x000118AC
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			using (SolidBrush b = new SolidBrush(Colors.GreyBackground))
			{
				g.FillRectangle(b, base.ClientRectangle);
			}
			using (Pen p = new Pen(Colors.DarkBorder))
			{
				g.DrawLine(p, base.ClientRectangle.Left, 0, base.ClientRectangle.Right, 0);
			}
			using (Pen p2 = new Pen(Colors.LightBorder))
			{
				g.DrawLine(p2, base.ClientRectangle.Left, 1, base.ClientRectangle.Right, 1);
			}
		}
	}
}
