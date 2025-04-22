using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;
using DarkUI.Extensions;

namespace DarkUI.Renderers
{
	// Token: 0x0200000F RID: 15
	public class DarkToolStripRenderer : DarkMenuRenderer
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00003E1C File Offset: 0x0000201C
		protected override void InitializeItem(ToolStripItem item)
		{
			base.InitializeItem(item);
			bool flag = item.GetType() == typeof(ToolStripSeparator);
			if (flag)
			{
				ToolStripSeparator castItem = (ToolStripSeparator)item;
				bool flag2 = !castItem.IsOnDropDown;
				if (flag2)
				{
					item.Margin = new Padding(0, 0, 2, 0);
				}
			}
			bool flag3 = item.GetType() == typeof(ToolStripButton);
			if (flag3)
			{
				item.AutoSize = false;
				item.Size = new Size(24, 24);
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003EA4 File Offset: 0x000020A4
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			Graphics bg = e.Graphics;
			using (SolidBrush b = new SolidBrush(Colors.BlackHighlight))
			{
				bg.FillRectangle(b, e.AffectedBounds);
			}
			Graphics g = e.Graphics;
			bool flag = e.ToolStrip.GetType() == typeof(ToolStripOverflow);
			if (flag)
			{
				using (Pen p = new Pen(Colors.GreySelection))
				{
					Rectangle rect = new Rectangle(e.AffectedBounds.Left, e.AffectedBounds.Top, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);
					g.DrawRectangle(p, rect);
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003F94 File Offset: 0x00002194
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			bool flag = e.ToolStrip.GetType() != typeof(ToolStrip);
			if (flag)
			{
				base.OnRenderToolStripBorder(e);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003FC8 File Offset: 0x000021C8
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 1, e.Item.Width, e.Item.Height - 2);
			bool flag = e.Item.Selected || e.Item.Pressed;
			if (flag)
			{
				using (SolidBrush b = new SolidBrush(Colors.GreySelection))
				{
					g.FillRectangle(b, rect);
				}
			}
			bool flag2 = e.Item.GetType() == typeof(ToolStripButton);
			if (flag2)
			{
				ToolStripButton castItem = (ToolStripButton)e.Item;
				bool @checked = castItem.Checked;
				if (@checked)
				{
					using (SolidBrush b2 = new SolidBrush(Colors.GreySelection))
					{
						g.FillRectangle(b2, rect);
					}
				}
				bool flag3 = castItem.Checked && castItem.Selected;
				if (flag3)
				{
					Rectangle modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
					using (Pen p = new Pen(Colors.GreyHighlight))
					{
						g.DrawRectangle(p, modRect);
					}
				}
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000413C File Offset: 0x0000233C
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 1, e.Item.Width, e.Item.Height - 2);
			bool flag = e.Item.Selected || e.Item.Pressed;
			if (flag)
			{
				using (SolidBrush b = new SolidBrush(Colors.GreySelection))
				{
					g.FillRectangle(b, rect);
				}
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000041C8 File Offset: 0x000023C8
		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			bool flag = e.GripStyle == ToolStripGripStyle.Hidden;
			if (!flag)
			{
				Graphics g = e.Graphics;
				using (Bitmap img = MenuIcons.grip.SetColor(Colors.LightBorder))
				{
					g.DrawImageUnscaled(img, new Point(e.AffectedBounds.Left, e.AffectedBounds.Top));
				}
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004244 File Offset: 0x00002444
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			Graphics g = e.Graphics;
			ToolStripSeparator castItem = (ToolStripSeparator)e.Item;
			bool isOnDropDown = castItem.IsOnDropDown;
			if (isOnDropDown)
			{
				base.OnRenderSeparator(e);
			}
			else
			{
				Rectangle rect = new Rectangle(3, 3, 2, e.Item.Height - 4);
				using (Pen p = new Pen(Colors.DarkBorder))
				{
					g.DrawLine(p, rect.Left, rect.Top, rect.Left, rect.Height);
				}
				using (Pen p2 = new Pen(Colors.LightBorder))
				{
					g.DrawLine(p2, rect.Left + 1, rect.Top, rect.Left + 1, rect.Height);
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004338 File Offset: 0x00002538
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			Graphics g = e.Graphics;
			bool flag = e.Image == null;
			if (!flag)
			{
				bool enabled = e.Item.Enabled;
				if (enabled)
				{
					g.DrawImageUnscaled(e.Image, new Point(e.ImageRectangle.Left, e.ImageRectangle.Top));
				}
				else
				{
					ControlPaint.DrawImageDisabled(g, e.Image, e.ImageRectangle.Left, e.ImageRectangle.Top, Color.Transparent);
				}
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000043C9 File Offset: 0x000025C9
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
		}
	}
}
