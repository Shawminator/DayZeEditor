using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Renderers
{
	// Token: 0x0200000E RID: 14
	public class DarkMenuRenderer : ToolStripRenderer
	{
		// Token: 0x06000051 RID: 81 RVA: 0x0000397A File Offset: 0x00001B7A
		protected override void Initialize(ToolStrip toolStrip)
		{
			base.Initialize(toolStrip);
			toolStrip.BackColor = Colors.BlackHighlight;
			toolStrip.ForeColor = Colors.LightText;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000039A0 File Offset: 0x00001BA0
		protected override void InitializeItem(ToolStripItem item)
		{
			base.InitializeItem(item);
			item.BackColor = Colors.BlackHighlight;
			item.ForeColor = Colors.BlueSelection;
			bool flag = item.GetType() == typeof(ToolStripSeparator);
			if (flag)
			{
				item.Margin = new Padding(0, 0, 0, 1);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000039FC File Offset: 0x00001BFC
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			Graphics g = e.Graphics;
			using (SolidBrush b = new SolidBrush(Colors.GreySelection))
			{
				g.FillRectangle(b, e.AffectedBounds);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003A4C File Offset: 0x00001C4C
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
			using (Pen p = new Pen(Colors.GreySelection))
			{
				g.DrawRectangle(p, rect);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003AB8 File Offset: 0x00001CB8
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(e.ImageRectangle.Left - 2, e.ImageRectangle.Top - 2, e.ImageRectangle.Width + 4, e.ImageRectangle.Height + 4);
			using (SolidBrush b = new SolidBrush(Colors.GreySelection))
			{
				g.FillRectangle(b, rect);
			}
			using (Pen p = new Pen(Colors.BlueHighlight))
			{
				Rectangle modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
				g.DrawRectangle(p, modRect);
			}
			bool flag = e.Item.ImageIndex == -1 && string.IsNullOrEmpty(e.Item.ImageKey) && e.Item.Image == null;
			if (flag)
			{
				g.DrawImageUnscaled(MenuIcons.tick, new Point(e.ImageRectangle.Left, e.ImageRectangle.Top));
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003C08 File Offset: 0x00001E08
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(1, 3, e.Item.Width, 1);
			using (SolidBrush b = new SolidBrush(Colors.GreySelection))
			{
				g.FillRectangle(b, rect);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003C68 File Offset: 0x00001E68
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			e.ArrowColor = Colors.LightText;
			e.ArrowRectangle = new Rectangle(new Point(e.ArrowRectangle.Left, e.ArrowRectangle.Top - 1), e.ArrowRectangle.Size);
			base.OnRenderArrow(e);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003CC8 File Offset: 0x00001EC8
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			Graphics g = e.Graphics;
			e.Item.ForeColor = (e.Item.Enabled ? Colors.LightText : Colors.DisabledText);
			bool enabled = e.Item.Enabled;
			if (enabled)
			{
				Color bgColor = (e.Item.Selected ? Colors.GreyBackground : e.Item.BackColor);
				Rectangle rect = new Rectangle(2, 0, e.Item.Width - 3, e.Item.Height);
				using (SolidBrush b = new SolidBrush(bgColor))
				{
					g.FillRectangle(b, rect);
				}
				bool flag = e.Item.GetType() == typeof(ToolStripMenuItem);
				if (flag)
				{
					bool flag2 = ((ToolStripMenuItem)e.Item).DropDown.Visible && !e.Item.IsOnDropDown;
					if (flag2)
					{
						using (SolidBrush b2 = new SolidBrush(Colors.GreySelection))
						{
							g.FillRectangle(b2, rect);
						}
					}
				}
			}
		}
	}
}
