using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000039 RID: 57
	public class DarkTitle : Label
	{
		// Token: 0x06000222 RID: 546 RVA: 0x0000CFAC File Offset: 0x0000B1AC
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height);
			SizeF textSize = g.MeasureString(this.Text, this.Font);
			using (SolidBrush b = new SolidBrush(Colors.LightText))
			{
				g.DrawString(this.Text, this.Font, b, new PointF(-2f, 0f));
			}
			using (Pen p = new Pen(Colors.GreySelection))
			{
				PointF p2 = new PointF(textSize.Width + 5f, textSize.Height / 2f);
				PointF p3 = new PointF((float)rect.Width, textSize.Height / 2f);
				g.DrawLine(p, p2, p3);
			}
		}
	}
}
